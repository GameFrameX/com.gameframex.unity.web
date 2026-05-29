using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrameX.Runtime;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        /// <summary>
        /// Web Binary请求数据类，用于处理Binary格式的Web请求
        /// </summary>
        private sealed class WebBinaryData : WebData
        {
            private Dictionary<string, string> m_Header;

            /// <summary>
            /// 获取请求任务的完成源，用于异步操作的控制和结果返回
            /// </summary>
            public TaskCompletionSource<WebBufferResult> Task { get; private set; }

            /// <summary>
            /// 获取要发送的字节数组数据
            /// </summary>
            public byte[] SendData { get; private set; }

            /// <summary>
            /// 获取请求头信息
            /// </summary>
            public Dictionary<string, string> Header { get { return m_Header; } }

            /// <summary>
            /// 无参构造函数（ReferencePool 需要new约束）
            /// </summary>
            public WebBinaryData() { }

            /// <summary>
            /// 创建Web Binary请求数据
            /// </summary>
            public static WebBinaryData Create(string url, Dictionary<string, string> header, byte[] sendData, TaskCompletionSource<WebBufferResult> task, object userData)
            {
                var data = ReferencePool.Acquire<WebBinaryData>();
                data.URL = url;
                data.IsGet = false;
                data.UserData = userData;
                data.Task = task;
                data.SendData = sendData;

                if (header != null && header.Count > 0)
                {
                    if (data.m_Header == null)
                    {
                        data.m_Header = new Dictionary<string, string>(header.Count);
                    }
                    else
                    {
                        data.m_Header.Clear();
                    }

                    foreach (var kv in header)
                    {
                        data.m_Header[kv.Key] = kv.Value;
                    }
                }

                return data;
            }

            public override void Clear()
            {
                if (Task != null)
                {
                    Task.TrySetCanceled();
                    Task = null;
                }

                SendData = null;

                if (m_Header != null)
                {
                    m_Header.Clear();
                }

                base.Clear();
            }
        }
    }
}
