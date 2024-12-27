using System.Threading.Tasks;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        private sealed class WebProtoBufData : WebData
        {
            /// <summary>
            /// 请求任务
            /// </summary>
            public readonly TaskCompletionSource<WebBufferResult> Task;

            /// <summary>
            /// 发送的数据
            /// </summary>
            public readonly byte[] SendData;

            public WebProtoBufData(string url, byte[] sendData, TaskCompletionSource<WebBufferResult> task, object userData) : base(false, url, userData)
            {
                task.CheckNull(nameof(task));
                SendData = sendData;
                Task = task;
            }
        }
    }
}