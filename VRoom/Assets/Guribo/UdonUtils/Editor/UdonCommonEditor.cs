#if !COMPILER_UDONSHARP && UNITY_EDITOR
using Guribo.UdonUtils.Runtime.Common;
using UnityEditor;

namespace Guribo.UdonUtils.Editor
{
    [CustomEditor(typeof(UdonCommon))]
    public class UdonCommonEditor : UdonLibraryEditor
    {
        protected override string GetSymbolName()
        {
            return "udonCommon";
        }
    }
}
#endif
