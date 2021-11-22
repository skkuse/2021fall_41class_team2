#if !COMPILER_UDONSHARP && UNITY_EDITOR
using Guribo.UdonUtils.Runtime.Common;
using UnityEditor;

namespace Guribo.UdonUtils.Editor
{
    [CustomEditor(typeof(UdonDebug))]
    public class UdonDebugEditor : UdonLibraryEditor
    {
        protected override string GetSymbolName()
        {
            return "udonDebug";
        }
    }
}
#endif
