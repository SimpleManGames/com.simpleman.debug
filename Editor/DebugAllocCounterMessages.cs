namespace Simplicity.Debug.Editor
{
    using System.Collections.Generic;
    using System.Linq;

    using Unity.CodeEditor;

    using UnityEditor;
    using UnityEditor.Build;

    public sealed class DebugAllocCounterMessages
    {
        private const string OUTPUT_MESSAGES = "Simplicity/Debug/AllocCounter/Output Logs";

        private const string DO_NOT_OUTPUT_MESSAGES = "Simplicity/Debug/AllocCounter/Don't Output Logs";

        private const string LOG_MESSAGES_KEY = "LogAllocCounter";

        private const string DEBUG_SYMBOL = "DEBUG_ENTER_EXIT_SCOPE";
        
        private static bool ShouldShowMessages
        {
            get
            {
                if (!EditorPrefs.HasKey(LOG_MESSAGES_KEY))
                {
                    EditorPrefs.SetBool(LOG_MESSAGES_KEY, true);
                }

                return EditorPrefs.GetBool(LOG_MESSAGES_KEY, true);
            }
            set => EditorPrefs.SetBool(LOG_MESSAGES_KEY, value);
        }

        [MenuItem(OUTPUT_MESSAGES, true, 0)]
        private static bool ShowOutputMessage()
        {
            bool shouldShowMessage = ShouldShowMessages;
            
            Menu.SetChecked(OUTPUT_MESSAGES, shouldShowMessage);
            return !shouldShowMessage;
        }

        [MenuItem(OUTPUT_MESSAGES, priority = 0)]
        private static void EnableOutputMessage()
        {
            ShouldShowMessages = true;
            NamedBuildTarget standalone = NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Standalone);
            List<string> definedSymbols = PlayerSettings.GetScriptingDefineSymbols(standalone).Split(";").ToList();
            definedSymbols.Add(DEBUG_SYMBOL);
            PlayerSettings.SetScriptingDefineSymbols(standalone, string.Join(';', definedSymbols));
            CodeEditor.CurrentEditor.SyncAll();
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }

        [MenuItem(DO_NOT_OUTPUT_MESSAGES, true, priority = 1)]
        private static bool ShowDoNotOutputMessage()
        {
            bool shouldShowMessage = ShouldShowMessages;
            
            Menu.SetChecked(DO_NOT_OUTPUT_MESSAGES, !shouldShowMessage);
            return shouldShowMessage;
        }

        [MenuItem(DO_NOT_OUTPUT_MESSAGES, priority = 1)]
        private static void DisableOutputMessage()
        {
            ShouldShowMessages = false;
            NamedBuildTarget standalone = NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Standalone);
            List<string> definedSymbols = PlayerSettings.GetScriptingDefineSymbols(standalone).Split(";").ToList();
            definedSymbols.Remove(DEBUG_SYMBOL);
            PlayerSettings.SetScriptingDefineSymbols(standalone, string.Join(';', definedSymbols));
            CodeEditor.CurrentEditor.SyncAll();
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }
    }
}
