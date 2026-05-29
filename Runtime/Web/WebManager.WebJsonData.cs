using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrameX.Runtime;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        /// <summary>
        /// Web JSON请求数据类，用于处理JSON格式的Web请求
        /// </summary>
        private sealed class WebJsonData : WebData
        {
            private Dictionary<string, string> m_Header;
            private Dictionary<string, object> m_Form;

            /// <summary>
            /// 获取请求头信息
            /// </summary>
            public Dictionary<string, string> Header { get { return m_Header; } }

            /// <summary>
            /// 获取表单数据
            /// </summary>
            public Dictionary<string, object> Form { get { return m_Form; } }

            /// <summary>
            /// 字符串结果的任务完成源
            /// </summary>
            public TaskCompletionSource<WebStringResult> UniTaskCompletionStringSource { get; private set; }

            /// <summary>
            /// 字节数组结果的任务完成源
            /// </summary>
            public TaskCompletionSource<WebBufferResult> UniTaskCompletionBytesSource { get; private set; }

            /// <summary>
            /// 无参构造函数（ReferencePool 需要new约束）
            /// </summary>
            public WebJsonData() { }

            /// <summary>
            /// 创建用于字节数组结果的GET/POST请求数据
            /// </summary>
            public static WebJsonData Create(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebBufferResult> source, object userData)
            {
                var data = ReferencePool.Acquire<WebJsonData>();
                data.URL = url;
                data.IsGet = isGet;
                data.UserData = userData;
                data.UniTaskCompletionBytesSource = source;
                CopyHeader(data, header);
                return data;
            }

            /// <summary>
            /// 创建用于字符串结果的GET/POST请求数据
            /// </summary>
            public static WebJsonData Create(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebStringResult> source, object userData)
            {
                var data = ReferencePool.Acquire<WebJsonData>();
                data.URL = url;
                data.IsGet = isGet;
                data.UserData = userData;
                data.UniTaskCompletionStringSource = source;
                CopyHeader(data, header);
                return data;
            }

            /// <summary>
            /// 创建用于带表单的字符串结果POST请求数据
            /// </summary>
            public static WebJsonData Create(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebStringResult> source, object userData)
            {
                var data = ReferencePool.Acquire<WebJsonData>();
                data.URL = url;
                data.IsGet = false;
                data.UserData = userData;
                data.UniTaskCompletionStringSource = source;
                CopyHeader(data, header);
                CopyForm(data, form);
                return data;
            }

            /// <summary>
            /// 创建用于带表单的字节数组结果POST请求数据
            /// </summary>
            public static WebJsonData Create(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebBufferResult> source, object userData)
            {
                var data = ReferencePool.Acquire<WebJsonData>();
                data.URL = url;
                data.IsGet = false;
                data.UserData = userData;
                data.UniTaskCompletionBytesSource = source;
                CopyHeader(data, header);
                CopyForm(data, form);
                return data;
            }

            public override void Clear()
            {
                if (UniTaskCompletionStringSource != null)
                {
                    UniTaskCompletionStringSource.TrySetCanceled();
                    UniTaskCompletionStringSource = null;
                }

                if (UniTaskCompletionBytesSource != null)
                {
                    UniTaskCompletionBytesSource.TrySetCanceled();
                    UniTaskCompletionBytesSource = null;
                }

                if (m_Header != null)
                {
                    m_Header.Clear();
                }

                if (m_Form != null)
                {
                    m_Form.Clear();
                }

                base.Clear();
            }

            private static void CopyHeader(WebJsonData data, Dictionary<string, string> header)
            {
                if (header == null || header.Count == 0)
                {
                    return;
                }

                if (data.m_Header == null)
                {
                    data.m_Header = new Dictionary<string, string>();
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

            private static void CopyForm(WebJsonData data, Dictionary<string, object> form)
            {
                if (form == null || form.Count == 0)
                {
                    return;
                }

                if (data.m_Form == null)
                {
                    data.m_Form = new Dictionary<string, object>();
                }
                else
                {
                    data.m_Form.Clear();
                }

                foreach (var kv in form)
                {
                    data.m_Form[kv.Key] = kv.Value;
                }
            }
        }
    }
}
