namespace GameFrameX.Web.Runtime
{
    public class WebStringResult
    {
        public WebStringResult(object userData, string result)
        {
            UserData = userData;
            Result = result;
        }

        /// <summary>
        /// 请求结果
        /// </summary>
        public string Result { get; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; }
    }
}