using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameFrameX.Web.Runtime
{
    public interface IWebManager
    {
        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> GetToString(string url, object userData = null);

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> GetToBytes(string url, object userData = null);

        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> GetToString(string url, Dictionary<string, string> queryString, object userData = null);


        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> GetToBytes(string url, Dictionary<string, string> queryString, object userData = null);


        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);


        /// <summary>
        /// 发送Get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);


        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> PostToString(string url, Dictionary<string, string> from, object userData = null);

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> PostToString(string url, Dictionary<string, string> from, Dictionary<string, string> queryString, object userData = null);

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebStringResult> PostToString(string url, Dictionary<string, string> from, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> PostToBytes(string url, Dictionary<string, string> from, object userData = null);

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> PostToBytes(string url, Dictionary<string, string> from, Dictionary<string, string> queryString, object userData = null);

        /// <summary>
        /// 发送Post 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="from">表单请求参数</param>
        /// <param name="queryString">URl请求参数</param>
        /// <param name="header">请求头</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns></returns>
        Task<WebBufferResult> PostToBytes(string url, Dictionary<string, string> from, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
    }
}