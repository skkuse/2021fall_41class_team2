#if !COMPILER_UDONSHARP && UNITY_EDITOR
using Guribo.UdonUtils.Runtime.Common;
using UnityEditor;

namespace Guribo.UdonUtils.Editor
{
    [CustomEditor(typeof(UdonMath))]
    public class UdonMathEditor : UdonLibraryEditor
    {
        protected override string GetSymbolName()
        {
            return "udonMath";
        }
    }
}
#endif
