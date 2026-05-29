using GameFrameX.Runtime;

namespace GameFrameX.Web.Runtime
{
    public partial class WebManager
    {
        /// <summary>
        /// Web请求数据的基类，包含请求的基本信息
        /// </summary>
        public class WebData : IReference
        {
            /// <summary>
            /// 获取用户自定义数据
            /// </summary>
            public object UserData { get; protected set; }

            /// <summary>
            /// 获取是否为GET请求
            /// </summary>
            public bool IsGet { get; protected set; }

            /// <summary>
            /// 获取请求URL
            /// </summary>
            public string URL { get; protected set; }

            /// <summary>
            /// 重置引用状态，将所有字段恢复为默认值
            /// </summary>
            public virtual void Clear()
            {
                UserData = default;
                IsGet = default;
                URL = default;
            }
        }
    }
}
