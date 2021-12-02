#if !COMPILER_UDONSHARP && UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Guribo.UdonUtils.Editor
{
    /// <summary>
    /// Adds the given define symbols to PlayerSettings define symbols.
    /// Just add your own define symbols to the Symbols property at the below.
    ///
    /// Original available under MIT License @
    /// https://github.com/UnityCommunity/UnityLibrary/blob/ac3ae833ee4b1636c521ca01b7e2d0c452fe37e7/Assets/Scripts/Editor/AddDefineSymbols.cs
    /// </summary>
    [InitializeOnLoad]
    public class CompileSymbols : UnityEditor.Editor
    {
        internal static readonly string EditorPreferencesEnableDebugLogging =
            "Guribo.UdonUtils.Editor.enableDebugLogging";

        public static readonly string DebugCompileSymbol = "GURIBO_DEBUG";

        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        static CompileSymbols()
        {
            UpdateSymbols();
        }

        private static void UpdateSymbols()
        {
            var definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();

            var result = new List<string>();

            if (EditorPrefs.GetBool(EditorPreferencesEnableDebugLogging, false))
            {
                if (!allDefines.Contains(DebugCompileSymbol))
                {
                    result.Add(DebugCompileSymbol);
                }
            }
            else
            {
                if (allDefines.Contains(DebugCompileSymbol))
                {
                    allDefines.Remove(DebugCompileSymbol);
                }
            }

            allDefines.AddRange(result.Except(allDefines));
            var uniqueEntries = new HashSet<string>(allDefines);
            allDefines = uniqueEntries.ToList();
            allDefines.Sort();

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }

        [MenuItem("Guribo/UdonUtils/Log Assertion Errors/Enable", true)]
        private static bool LogAssertionErrorsEnableValidation()
        {
            return !EditorPrefs.GetBool(EditorPreferencesEnableDebugLogging, false);
        }

        [MenuItem("Guribo/UdonUtils/Log Assertion Errors/Enable", false, 2)]
        public static void LogAssertionErrorsEnable()
        {
            EditorPrefs.SetBool(EditorPreferencesEnableDebugLogging, true);
            InteractiveRefresh(true);
        }

        [MenuItem("Guribo/UdonUtils/Log Assertion Errors/Disable", true)]
        private static bool LogAssertionErrorsDisableValidation()
        {
            return EditorPrefs.GetBool(EditorPreferencesEnableDebugLogging, false);
        }

        [MenuItem("Guribo/UdonUtils/Log Assertion Errors/Disable", false, 1)]
        public static void LogAssertionErrorsDisable()
        {
            if (EditorPrefs.HasKey(EditorPreferencesEnableDebugLogging))
            {
                EditorPrefs.DeleteKey(EditorPreferencesEnableDebugLogging);
            }

            InteractiveRefresh(false);
        }

        private static void InteractiveRefresh(bool enabled)
        {
            UpdateSymbols();
            var newState = enabled ? "enabled" : "disabled";
            EditorUtility.DisplayDialog("Info", $"Debug Logging {newState}.\nScripts will be recompiled now.", "Ok");
            AssetDatabase.Refresh();
        }
    }
}
#endif