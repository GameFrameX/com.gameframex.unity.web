using GameFrameX.Editor;
using GameFrameX.Web.Runtime;
using UnityEditor;

namespace GameFrameX.Web.Editor
{
    [CustomEditor(typeof(WebComponent))]
    internal sealed class WebComponentInspector : ComponentTypeComponentInspector
    {
        protected override void RefreshTypeNames()
        {
            base.RefreshTypeNames();
            RefreshComponentTypeNames(typeof(IWebManager));
        }
    }
}