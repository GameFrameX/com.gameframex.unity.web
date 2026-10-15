using UnityEngine;

namespace GameFrameX.Web.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public class GameFrameXWebCroppingHelper : MonoBehaviour
    {
        [UnityEngine.Scripting.Preserve]
        private void Start()
        {
            _ = typeof(WebComponent);
            _ = typeof(IWebManager);
            _ = typeof(WebManager);
            _ = typeof(WebStringResult);
            _ = typeof(WebBufferResult);
        }
    }
}