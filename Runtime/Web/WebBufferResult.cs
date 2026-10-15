namespace GameFrameX.Web.Runtime
{
    public sealed class WebBufferResult
    {
        [UnityEngine.Scripting.Preserve]
        public WebBufferResult(object userData, byte[] result)
        {
            UserData = userData;
            Result = result;
        }

        /// <summary>
        /// 请求结果
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public byte[] Result { get; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public object UserData { get; }
    }
}