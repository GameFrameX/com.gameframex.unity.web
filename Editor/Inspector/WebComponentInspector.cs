using System;
using System.IO;
using System.Text;
using GameFrameX.Editor;
using GameFrameX.Web.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.Web.Editor
{
    [CustomEditor(typeof(WebComponent))]
    internal sealed class WebComponentInspector : GameFrameworkInspector
    {
        // private SerializedProperty m_InstanceRoot = null;
        // private SerializedProperty m_WebRequestAgentHelperCount = null;
        // private SerializedProperty m_Timeout = null;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();


            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            // m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            // m_WebRequestAgentHelperCount = serializedObject.FindProperty("m_WebRequestAgentHelperCount");
            // m_Timeout = serializedObject.FindProperty("m_Timeout");
            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}