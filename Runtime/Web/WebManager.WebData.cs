using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        private sealed class WebData
        {
            public object UserData { get; }
            public bool IsGet { get; }
            public string URL { get; set; }
            public Dictionary<string, string> Header { get; }
            public Dictionary<string, object> Form { get; }
            public readonly TaskCompletionSource<WebStringResult> UniTaskCompletionStringSource;
            public readonly TaskCompletionSource<WebBufferResult> UniTaskCompletionBytesSource;

            public WebData(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebBufferResult> source, object userData = null)
            {
                IsGet = isGet;
                URL = url;
                Header = header;
                UniTaskCompletionBytesSource = source;
                UserData = userData;
            }

            public WebData(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebStringResult> source, object userData = null)
            {
                IsGet = isGet;
                URL = url;
                Header = header;
                UniTaskCompletionStringSource = source;
                UserData = userData;
            }

            public WebData(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebStringResult> source, object userData = null)
            {
                IsGet = false;
                URL = url;
                Header = header;
                Form = form;
                UniTaskCompletionStringSource = source;
                UserData = userData;
            }

            public WebData(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebBufferResult> source, object userData = null)
            {
                IsGet = false;
                URL = url;
                Header = header;
                Form = form;
                UniTaskCompletionBytesSource = source;
                UserData = userData;
            }
        }
    }
}