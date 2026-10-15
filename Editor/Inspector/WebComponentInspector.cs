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
            RefreshComponentTypeNames(typeof(IWebManager));
        }

        public override void OnInspectorGUI()
        {

            // C9 活视图：编辑态提示 Options 入口；Play 态实时显示/修改超时（经 WebComponent.Timeout 公开属性写回管理器）。
            GameFrameXOptionsLiveViewUtility.DrawLiveView((WebComponent)target, typeof(WebOptions));
            base.OnInspectorGUI();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        protected override void Enable()
        {
            base.Enable();
            serializedObject.ApplyModifiedProperties();
        }
    }
}