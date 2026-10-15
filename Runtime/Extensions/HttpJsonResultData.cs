namespace GameFrameX.Web.Runtime
{
    /// <summary>
    /// 消息返回统一结构
    /// 该类用于封装HTTP请求的返回结果，提供统一的结构以便于处理和解析响应数据。
    /// </summary>
    /// <typeparam name="T">消息类型，表示返回的数据对象的类型。</typeparam>
    public sealed class HttpJsonResultData<T>
    {
        /// <summary>
        /// 是否成功
        /// 表示请求是否成功执行，成功为true，失败为false。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 响应码
        /// 表示请求的处理结果，为0表示成功，其他值表示不同的错误类型。
        /// 服务器错误（如 HTTP 500）无法识别具体错误码时固定为 500。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// 请求失败时为服务端返回的错误消息（如 HTTP 500 错误体的 message/error/title）；
        /// 请求成功时为服务端返回的原始消息，可能为空。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string Message { get; set; }

        /// <summary>
        /// 数据对象
        /// 包含请求成功时返回的数据，类型为T。
        /// 如果请求失败，可能为默认值或null。
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public T Data { get; set; }
    }
}