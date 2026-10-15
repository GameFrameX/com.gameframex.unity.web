using System;
using GameFrameX.Runtime;
using GameFrameX.LitJSON.Runtime;

namespace GameFrameX.Web.Runtime
{
    /// <summary>
    /// 提供用于处理HTTP JSON结果的辅助方法。
    /// </summary>
    public static class HttpJsonResultHelper
    {
        /// <summary>
        /// 响应体无法解析为有效结果时使用的固定错误码（对应 HTTP 500 Internal Server Error）。
        /// </summary>
        private const int ServerErrorCode = 500;

        /// <summary>
        /// 将JSON字符串转换为HttpJsonResultData&lt;T&gt;对象。
        /// 该方法尝试反序列化给定的JSON字符串，并根据HTTP响应的状态码设置IsSuccess属性。
        /// 如果响应成功，Data属性将包含反序列化后的数据对象；否则，Data将为默认值。
        /// 服务器或网关返回错误（如 HTTP 500）时，响应体可能不是约定的 code 结构：
        /// 此时回退识别常见的错误结构（status/statusCode + message/error/title），
        /// 识别失败则视为失败并将 Code 置为 500；响应体为空或非 JSON（如 HTML 错误页）时同样置为 500。
        /// </summary>
        /// <typeparam name="T">要反序列化为的对象类型，必须是类并具有无参数构造函数。</typeparam>
        /// <param name="jsonResult">包含HTTP响应的JSON字符串。</param>
        /// <returns>HttpJsonResultData&lt;T&gt;对象，表示反序列化的结果。</returns>
        [UnityEngine.Scripting.Preserve]
        public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult) where T : class, new()
        {
            HttpJsonResultData<T> resultData = new HttpJsonResultData<T>
            {
                IsSuccess = false,
            };
            try
            {
                if (string.IsNullOrEmpty(jsonResult))
                {
                    // 空响应：服务器异常（如 500 空响应体）或网关异常
                    resultData.Code = ServerErrorCode;
                    resultData.Message = "Server response is empty.";
                    return resultData;
                }

                // 反序列化JSON字符串为HttpJsonResult对象
                var httpJsonResult = JsonMapper.ToObject<HttpJsonResult>(jsonResult);
                if (httpJsonResult == null)
                {
                    // 响应体不是 JSON 对象（如 JSON 字面量 null）
                    resultData.Code = ServerErrorCode;
                    resultData.Message = "Server response is not a valid JSON object.";
                    return resultData;
                }

                // 检查约定的业务响应码是否表示成功
                if (httpJsonResult.Code != 0)
                {
                    resultData.Code = httpJsonResult.Code;
                    resultData.Message = httpJsonResult.Message;
                    return resultData; // 返回默认的失败结果
                }

                // 服务器/网关返回错误（如 HTTP 500）时响应体可能缺少约定的 code 字段，
                // 回退识别常见错误结构：ASP.NET Core ProblemDetails（status/title）、
                // 旧版 statusCode、Spring Boot 默认错误体（status/error）
                if (jsonResult.IndexOf("status", StringComparison.OrdinalIgnoreCase) >= 0 && TryGetServerError(jsonResult, out var errorCode, out var errorMessage))
                {
                    resultData.Code = errorCode;
                    resultData.Message = errorMessage;
                    return resultData;
                }

                resultData.IsSuccess = true; // 设置成功标志
                resultData.Message = httpJsonResult.Message;
                // 反序列化数据部分，如果数据为空则返回类型T的默认实例
                resultData.Data = string.IsNullOrEmpty(httpJsonResult.Data) ? new T() : JsonMapper.ToObject<T>(httpJsonResult.Data);
            }
            catch (Exception e)
            {
                // 捕获并输出异常信息（如服务器返回 HTML 错误页导致反序列化失败）
                Log.Error(e);
                resultData.IsSuccess = false;
                resultData.Code = ServerErrorCode;
                resultData.Message = e.Message;
            }

            return resultData; // 返回结果数据
        }

        /// <summary>
        /// 尝试从响应体中识别常见的服务器错误结构（如 HTTP 500 错误体）。
        /// </summary>
        /// <param name="jsonResult">包含HTTP响应的JSON字符串。</param>
        /// <param name="code">识别到的错误码（status/statusCode 字段值）。</param>
        /// <param name="message">识别到的错误消息（message/error/title 字段值）。</param>
        /// <returns>识别到服务器错误时返回true；否则返回false。</returns>
        private static bool TryGetServerError(string jsonResult, out int code, out string message)
        {
            code = 0;
            message = null;

            JsonData jsonData = JsonMapper.ToObject(jsonResult);
            if (jsonData == null || !jsonData.IsObject)
            {
                return false;
            }

            code = GetNonZeroNumber(jsonData, "statusCode");
            if (code == 0)
            {
                code = GetNonZeroNumber(jsonData, "status");
            }

            if (code == 0)
            {
                return false;
            }

            message = GetFirstNonEmptyString(jsonData, "message", "error", "title");
            return true;
        }

        /// <summary>
        /// 获取指定名称的非零数值字段（非数值类型视为不存在）。
        /// </summary>
        private static int GetNonZeroNumber(JsonData jsonData, string name)
        {
            if (!TryGetValue(jsonData, name, out JsonData value) || (!value.IsInt && !value.IsLong))
            {
                return 0;
            }

            return (int)value;
        }

        /// <summary>
        /// 依次获取第一个非空字符串字段。
        /// </summary>
        private static string GetFirstNonEmptyString(JsonData jsonData, params string[] names)
        {
            foreach (string name in names)
            {
                if (TryGetValue(jsonData, name, out JsonData value) && value.IsString && !string.IsNullOrEmpty((string)value))
                {
                    return (string)value;
                }
            }

            return null;
        }

        /// <summary>
        /// 按名称（忽略大小写）获取JSON对象字段。
        /// </summary>
        private static bool TryGetValue(JsonData jsonData, string name, out JsonData value)
        {
            value = null;
            foreach (string key in jsonData.Keys)
            {
                if (string.Equals(key, name, StringComparison.OrdinalIgnoreCase))
                {
                    value = jsonData[key];
                    return true;
                }
            }

            return false;
        }
    }
}