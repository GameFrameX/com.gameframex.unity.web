using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        private sealed class WebJsonData : WebData
        {
            public Dictionary<string, string> Header { get; }
            public Dictionary<string, object> Form { get; }
            public readonly TaskCompletionSource<WebStringResult> UniTaskCompletionStringSource;
            public readonly TaskCompletionSource<WebBufferResult> UniTaskCompletionBytesSource;

            public WebJsonData(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebBufferResult> source, object userData = null) : base(isGet, url, userData)
            {
                Header = header;
                UniTaskCompletionBytesSource = source;
            }

            public WebJsonData(string url, Dictionary<string, string> header, bool isGet, TaskCompletionSource<WebStringResult> source, object userData = null) : base(isGet, url, userData)
            {
                Header = header;
                UniTaskCompletionStringSource = source;
            }

            public WebJsonData(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebStringResult> source, object userData = null) : base(false, url, userData)
            {
                Header = header;
                Form = form;
                UniTaskCompletionStringSource = source;
            }

            public WebJsonData(string url, Dictionary<string, string> header, Dictionary<string, object> form, TaskCompletionSource<WebBufferResult> source, object userData = null) : base(false, url, userData)
            {
                Header = header;
                Form = form;
                UniTaskCompletionBytesSource = source;
            }

            public override void Dispose()
            {
                if (UniTaskCompletionStringSource != null)
                {
                    UniTaskCompletionStringSource.TrySetCanceled();
                }

                if (UniTaskCompletionBytesSource != null)
                {
                    UniTaskCompletionBytesSource.TrySetCanceled();
                }

                base.Dispose();
            }
        }
    }
}