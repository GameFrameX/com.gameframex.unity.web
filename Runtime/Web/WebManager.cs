using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GameFrameX.Runtime;
#if UNITY_WEBGL
using UnityEngine.Networking;
#endif

namespace GameFrameX.Web.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public partial class WebManager : GameFrameworkModule, IWebManager
    {
        private readonly StringBuilder m_StringBuilder = new StringBuilder(256);
        private readonly Queue<WebData> m_WaitingQueue = new Queue<WebData>(256);
        private readonly List<WebData> m_SendingList = new List<WebData>(16);
        private readonly MemoryStream m_MemoryStream;
        private float m_Timeout = 5f;

        [UnityEngine.Scripting.Preserve]
        public WebManager()
        {
            MaxConnectionPerServer = 8;
            m_MemoryStream = new MemoryStream();
            Timeout = 5f;
        }

        public float Timeout
        {
            get { return m_Timeout; }
            set
            {
                m_Timeout = value;
                RequestTimeout = TimeSpan.FromSeconds(value);
            }
        }

        public int MaxConnectionPerServer { get; set; }

        public TimeSpan RequestTimeout { get; set; }

        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
            lock (m_StringBuilder)
            {
                if (m_SendingList.Count < MaxConnectionPerServer)
                {
                    if (m_WaitingQueue.Count > 0)
                    {
                        var webData = m_WaitingQueue.Dequeue();

                        if (webData.UniTaskCompletionStringSource != null)
                        {
                            MakeStringRequest(webData);
                        }
                        else
                        {
                            MakeBytesRequest(webData);
                        }

                        m_SendingList.Add(webData);
                    }
                }
            }
        }

        protected override void Shutdown()
        {
            while (m_WaitingQueue.Count > 0)
            {
                var webData = m_WaitingQueue.Dequeue();
                webData.UniTaskCompletionBytesSource?.TrySetCanceled();
                webData.UniTaskCompletionStringSource?.TrySetCanceled();
            }

            m_WaitingQueue.Clear();
            while (m_SendingList.Count > 0)
            {
                var webData = m_SendingList[0];
                m_SendingList.RemoveAt(0);
                webData.UniTaskCompletionBytesSource?.TrySetCanceled();
                webData.UniTaskCompletionStringSource?.TrySetCanceled();
            }

            m_SendingList.Clear();
            m_MemoryStream.Dispose();
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> GetToString(string url, object userData = null)
        {
            return GetToString(url, null, null, userData);
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> GetToBytes(string url, object userData = null)
        {
            return GetToBytes(url, null, null, userData);
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> GetToString(string url, Dictionary<string, string> queryString, object userData = null)
        {
            return GetToString(url, queryString, null, userData);
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> GetToBytes(string url, Dictionary<string, string> queryString, object userData = null)
        {
            return GetToBytes(url, queryString, null, userData);
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null)
        {
            var uniTaskCompletionSource = new TaskCompletionSource<WebStringResult>();
            url = UrlHandler(url, queryString);

            WebData webData = new WebData(url, header, true, uniTaskCompletionSource, userData);
            m_WaitingQueue.Enqueue(webData);
            return uniTaskCompletionSource.Task;
        }

        private async void MakeStringRequest(WebData webData)
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
            if (webData.Form != null && webData.Form.Count > 0)
            {
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                string body = Utility.Json.ToJson(webData.Form);
                byte[] postData = Encoding.UTF8.GetBytes(body);
                unityWebRequest.uploadHandler = new UploadHandlerRaw(postData);
            }

            if (webData.Header != null && webData.Header.Count > 0)
            {
                foreach (var kv in webData.Header)
                {
                    unityWebRequest.SetRequestHeader(kv.Key, kv.Value);
                }
            }

            var asyncOperation = unityWebRequest.SendWebRequest();
            asyncOperation.completed += (asyncOperation2) =>
            {
                m_SendingList.Remove(webData);
                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError || unityWebRequest.error != null)
                {
                    webData.UniTaskCompletionStringSource.TrySetException(new Exception(unityWebRequest.error));
                    return;
                }

                webData.UniTaskCompletionStringSource.SetResult(new WebStringResult(webData.UserData, unityWebRequest.downloadHandler.text));
            };
#else
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(webData.URL);
                request.Method = webData.IsGet ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
                request.Timeout = (int)RequestTimeout.TotalMilliseconds; // 设置请求超时时间
                if (webData.Form != null && webData.Form.Count > 0)
                {
                    request.ContentType = "application/json";
                    string body = Utility.Json.ToJson(webData.Form);
                    byte[] postData = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = postData.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        await requestStream.WriteAsync(postData, 0, postData.Length);
                    }
                }

                if (webData.Header != null && webData.Header.Count > 0)
                {
                    foreach (var kv in webData.Header)
                    {
                        request.Headers[kv.Key] = kv.Value;
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string content = await reader.ReadToEndAsync();

                        m_SendingList.Remove(webData);
                        webData.UniTaskCompletionStringSource.SetResult(new WebStringResult(webData.UserData, content));
                    }
                }
            }
            catch (WebException e)
            {
                m_SendingList.Remove(webData);

                // 捕获超时异常
                if (e.Status == WebExceptionStatus.Timeout)
                {
                    webData.UniTaskCompletionStringSource.SetException(new TimeoutException(e.Message));
                    return;
                }

                webData.UniTaskCompletionStringSource.SetException(e);
            }
            catch (IOException e)
            {
                m_SendingList.Remove(webData);
                webData.UniTaskCompletionStringSource.SetException(e);
            }
            catch (Exception e)
            {
                m_SendingList.Remove(webData);
                webData.UniTaskCompletionStringSource.SetException(e);
            }
#endif
        }

        private async void MakeBytesRequest(WebData webData)
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
            if (webData.Form != null && webData.Form.Count > 0)
            {
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                string body = Utility.Json.ToJson(webData.Form);
                byte[] postData = Encoding.UTF8.GetBytes(body);
                unityWebRequest.uploadHandler = new UploadHandlerRaw(postData);
            }

            if (webData.Header != null && webData.Header.Count > 0)
            {
                foreach (var kv in webData.Header)
                {
                    unityWebRequest.SetRequestHeader(kv.Key, kv.Value);
                }
            }

            var asyncOperation = unityWebRequest.SendWebRequest();
            asyncOperation.completed += (asyncOperation2) =>
            {
                m_SendingList.Remove(webData);
                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError || unityWebRequest.error != null)
                {
                    webData.UniTaskCompletionBytesSource.TrySetException(new Exception(unityWebRequest.error));
                    return;
                }

                webData.UniTaskCompletionBytesSource.SetResult(new WebBufferResult(webData.UserData, unityWebRequest.downloadHandler.data));
            };
#else
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(webData.URL);
                request.Method = webData.IsGet ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
                request.Timeout = (int)RequestTimeout.TotalMilliseconds; // 设置请求超时时间
                if (webData.Header != null && webData.Header.Count > 0)
                {
                    foreach (var kv in webData.Header)
                    {
                        request.Headers[kv.Key] = kv.Value;
                    }
                }

                if (webData.Form != null && webData.Form.Count > 0)
                {
                    request.ContentType = "application/json";
                    string body = Utility.Json.ToJson(webData.Form);
                    byte[] postData = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = postData.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        await requestStream.WriteAsync(postData, 0, postData.Length);
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        await responseStream.CopyToAsync(m_MemoryStream);
                        webData.UniTaskCompletionBytesSource.SetResult(new WebBufferResult(webData.UserData, m_MemoryStream.ToArray())); // 将流的内容复制到内存流中并转换为byte数组 
                    }
                }
            }
            catch (WebException e)
            {
                m_SendingList.Remove(webData);
                // 捕获超时异常
                if (e.Status == WebExceptionStatus.Timeout)
                {
                    webData.UniTaskCompletionBytesSource.SetException(new TimeoutException(e.Message));
                    return;
                }

                webData.UniTaskCompletionBytesSource.SetException(e);
            }
            catch (IOException e)
            {
                m_SendingList.Remove(webData);
                webData.UniTaskCompletionBytesSource.SetException(e);
            }
            catch (Exception e)
            {
                m_SendingList.Remove(webData);
                webData.UniTaskCompletionBytesSource.SetException(e);
            }
#endif
        }

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null)
        {
            var uniTaskCompletionSource = new TaskCompletionSource<WebBufferResult>();
            url = UrlHandler(url, queryString);

            WebData webData = new WebData(url, header, true, uniTaskCompletionSource, userData);
            m_WaitingQueue.Enqueue(webData);
            return uniTaskCompletionSource.Task;
        }


        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> PostToString(string url, Dictionary<string, object> from, object userData = null)
        {
            return PostToString(url, from, null, null, userData);
        }

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> PostToBytes(string url, Dictionary<string, object> from, object userData = null)
        {
            return PostToBytes(url, from, null, null, userData);
        }

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> PostToString(string url, Dictionary<string, object> from, Dictionary<string, string> queryString, object userData = null)
        {
            return PostToString(url, from, queryString, null, userData);
        }

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> PostToBytes(string url, Dictionary<string, object> from, Dictionary<string, string> queryString, object userData = null)
        {
            return PostToBytes(url, from, queryString, null, userData);
        }


        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebStringResult> PostToString(string url, Dictionary<string, object> from, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null)
        {
            var uniTaskCompletionSource = new TaskCompletionSource<WebStringResult>();
            url = UrlHandler(url, queryString);

            WebData webData = new WebData(url, header, from, uniTaskCompletionSource, userData);
            m_WaitingQueue.Enqueue(webData);
            return uniTaskCompletionSource.Task;
        }

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public Task<WebBufferResult> PostToBytes(string url, Dictionary<string, object> from, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null)
        {
            var uniTaskCompletionSource = new TaskCompletionSource<WebBufferResult>();
            url = UrlHandler(url, queryString);
            WebData webData = new WebData(url, header, from, uniTaskCompletionSource, userData);
            m_WaitingQueue.Enqueue(webData);
            return uniTaskCompletionSource.Task;
        }


        /// <summary>
        /// URL 标准化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private string UrlHandler(string url, Dictionary<string, string> queryString)
        {
            m_StringBuilder.Clear();
            m_StringBuilder.Append((url));
            if (queryString != null && queryString.Count > 0)
            {
                if (!url.EndsWithFast("?"))
                {
                    m_StringBuilder.Append("?");
                }

                foreach (var kv in queryString)
                {
                    m_StringBuilder.AppendFormat("{0}={1}&", kv.Key, kv.Value);
                }

                url = m_StringBuilder.ToString(0, m_StringBuilder.Length - 1);
                m_StringBuilder.Clear();
            }

            return url;
        }
    }
}