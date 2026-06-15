using GameFrameX.LitJSON.Runtime;

namespace GameFrameX.Web.Runtime
{
    /// <summary>
    /// HTTP网页请求的消息响应结构
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public sealed class HttpJsonResult
    {
        /// <summary>
        /// 响应码0 为成功
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 响应数据.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        public override string ToString()
        {
            return JsonMapper.ToJson(this);
        }
    }
}