using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        private class WebData : IDisposable
        {
            public object UserData { get; }
            public bool IsGet { get; }
            public string URL { get; }

            protected WebData(bool isGet, string url, object userData = null)
            {
                UserData = userData;
                IsGet = isGet;
                URL = url;
            }

            public virtual void Dispose()
            {
            }
        }
    }
}