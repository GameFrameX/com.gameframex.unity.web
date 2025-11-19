// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
// 
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
// 
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
// 
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
// 
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Editor;
using UnityEditor;

namespace GameFrameX.Web.Editor
{
    /// <summary>
    /// 网络日志脚本宏定义。
    /// </summary>
    public static class WebLogScriptingDefineSymbols
    {
        public const string EnableNetworkReceiveLogScriptingDefineSymbol = "ENABLE_GAMEFRAMEX_WEB_RECEIVE_LOG";
        public const string EnableNetworkSendLogScriptingDefineSymbol = "ENABLE_GAMEFRAMEX_WEB_SEND_LOG";

        /// <summary>
        /// 禁用Web接收日志脚本宏定义。
        /// </summary>
        [MenuItem("GameFrameX/Scripting Define Symbols/Disable Web Receive Logs(关闭Web接收日志打印)", false, 460)]
        public static void DisableNetworkReceiveLogs()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableNetworkReceiveLogScriptingDefineSymbol);
        }

        /// <summary>
        /// 开启Web接收日志脚本宏定义。
        /// </summary>
        [MenuItem("GameFrameX/Scripting Define Symbols/Enable Web Receive Logs(开启Web接收日志打印)", false, 461)]
        public static void EnableNetworkReceiveLogs()
        {
            ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableNetworkReceiveLogScriptingDefineSymbol);
        }

        /// <summary>
        /// 禁用Web发送日志脚本宏定义。
        /// </summary>
        [MenuItem("GameFrameX/Scripting Define Symbols/Disable Web Send Logs(关闭Web发送日志打印)", false, 450)]
        public static void DisableNetworkSendLogs()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableNetworkSendLogScriptingDefineSymbol);
        }

        /// <summary>
        /// 开启Web发送日志脚本宏定义。
        /// </summary>
        [MenuItem("GameFrameX/Scripting Define Symbols/Enable Web Send Logs(开启Web发送日志打印)", false, 451)]
        public static void EnableNetworkSendLogs()
        {
            ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableNetworkSendLogScriptingDefineSymbol);
        }
    }
}