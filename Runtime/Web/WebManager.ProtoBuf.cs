using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GameFrameX.Network.Runtime;
using GameFrameX.Runtime;
using ProtoBuf;
#if UNITY_WEBGL
using UnityEngine.Networking;
#endif

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager : GameFrameworkModule, IWebManager
    {
        private readonly Queue<WebProtoBufData> m_WaitingProtoBufQueue = new Queue<WebProtoBufData>(256);
        private readonly List<WebProtoBufData> m_SendingProtoBufList = new List<WebProtoBufData>(16);

        private const string ProtoBufContentType = "application/x-protobuf";


        void UpdateProtoBuf(float elapseSeconds, float realElapseSeconds)
        {
            lock (m_StringBuilder)
            {
                if (m_SendingProtoBufList.Count < MaxConnectionPerServer)
                {
                    if (m_WaitingProtoBufQueue.Count > 0)
                    {
                        var webProtoBufData = m_WaitingProtoBufQueue.Dequeue();

                        MakeProtoBufBytesRequest(webProtoBufData);

                        m_SendingProtoBufList.Add(webProtoBufData);
                    }
                }
            }
        }

        private void ShutdownProtoBuf()
        {
            while (m_WaitingProtoBufQueue.Count > 0)
            {
                var webData = m_WaitingProtoBufQueue.Dequeue();
                webData.Dispose();
            }

            m_WaitingProtoBufQueue.Clear();
            while (m_SendingProtoBufList.Count > 0)
            {
                var webData = m_SendingProtoBufList[0];
                m_SendingProtoBufList.RemoveAt(0);
                webData.Dispose();
            }

            m_SendingProtoBufList.Clear();

            m_MemoryStream.Dispose();
        }


        private async void MakeProtoBufBytesRequest(WebProtoBufData webData)
        {
#if UNITY_WEBGL
            UnityWebRequest unityWebRequest;
            if (webData.IsGet)
            {
                unityWebRequest = UnityWebRequest.Get(webData.URL);
            }
            else
            {
                unityWebRequest = UnityWebRequest.Post(webData.URL, string.Empty);
            }

            unityWebRequest.timeout = (int)RequestTimeout.TotalSeconds;
            {
                unityWebRequest.SetRequestHeader("Content-Type", ProtoBufContentType);
                byte[] postData = webData.SendData;
                unityWebRequest.uploadHandler = new UploadHandlerRaw(postData);
            }

            var asyncOperation = unityWebRequest.SendWebRequest();
            asyncOperation.completed += (asyncOperation2) =>
            {
                m_SendingProtoBufList.Remove(webData);
                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError || unityWebRequest.error != null)
                {
                    webData.Task.TrySetException(new Exception(unityWebRequest.error));
                    return;
                }

                webData.Task.SetResult(new WebBufferResult(webData.UserData, unityWebRequest.downloadHandler.data));
            };
#else
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(webData.URL);
                request.Method = webData.IsGet ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
                request.Timeout = (int)RequestTimeout.TotalMilliseconds; // 设置请求超时时间
                request.ContentType = ProtoBufContentType;
                byte[] postData = webData.SendData;
                request.ContentLength = postData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(postData, 0, postData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        m_MemoryStream.SetLength(responseStream.Length);
                        m_MemoryStream.Position = 0;
                        await responseStream.CopyToAsync(m_MemoryStream);
                        webData.Task.SetResult(new WebBufferResult(webData.UserData, m_MemoryStream.ToArray())); // 将流的内容复制到内存流中并转换为byte数组 
                    }
                }
            }
            catch (WebException e)
            {
                // 捕获超时异常
                if (e.Status == WebExceptionStatus.Timeout)
                {
                    webData.Task.SetException(new TimeoutException(e.Message));
                    return;
                }

                webData.Task.SetException(e);
            }
            catch (IOException e)
            {
                webData.Task.SetException(e);
            }
            catch (Exception e)
            {
                webData.Task.SetException(e);
            }
            finally
            {
                m_SendingProtoBufList.Remove(webData);
            }
#endif
        }


        /// <summary>
        /// 发送Post请求。
        /// </summary>
        /// <param name="url">目标服务器的URL地址。</param>
        /// <param name="message">要发送的消息对象，必须继承自MessageObject。</param>
        /// <typeparam name="T">返回的数据类型，必须继承自MessageObject并且实现IResponseMessage接口。</typeparam>
        /// <returns>返回一个任务对象，该任务完成时将包含从服务器接收到的响应数据，数据类型为T。</returns>
        /// <remarks>
        /// 此方法用于向指定的URL发送POST请求，并接收响应。请求的消息体由参数message提供，而响应则会被解析为指定的泛型类型T。
        /// </remarks>
        public async Task<T> Post<T>(string url, MessageObject message) where T : MessageObject, IResponseMessage
        {
            var webBufferResult = await PostInner(url, message);
            if (webBufferResult.IsNotNull())
            {
                var messageObjectHttp = SerializerHelper.Deserialize<MessageHttpObject>(webBufferResult.Result);
                if (messageObjectHttp.IsNotNull() && messageObjectHttp.Id != default)
                {
                    var messageType = ProtoMessageIdHandler.GetRespTypeById(messageObjectHttp.Id);
                    if (messageType != typeof(T))
                    {
                        Log.Error($"Response message type is invalid. Expected '{typeof(T).FullName}', actual '{messageType.FullName}'.");
                        return default;
                    }

                    return SerializerHelper.Deserialize<T>(messageObjectHttp.Body);
                }
            }

            return default;
        }

        private Task<WebBufferResult> PostInner(string url, MessageObject message, object userData = null)
        {
            var uniTaskCompletionSource = new TaskCompletionSource<WebBufferResult>();
            url = UrlHandler(url, null);
            var id = ProtoMessageIdHandler.GetReqMessageIdByType(message.GetType());
            MessageHttpObject messageHttpObject = new MessageHttpObject
            {
                Id = id,
                UniqueId = message.UniqueId,
                Body = SerializerHelper.Serialize(message),
            };
            var sendData = SerializerHelper.Serialize(messageHttpObject);
            var webData = new WebProtoBufData(url, sendData, uniTaskCompletionSource, userData);
            m_WaitingProtoBufQueue.Enqueue(webData);
            return uniTaskCompletionSource.Task;
        }
    }
}