#if !COMPILER_UDONSHARP && UNITY_EDITOR
using Guribo.UdonUtils.Runtime.Common.Networking;
using UnityEditor;

namespace Guribo.UdonUtils.Editor
{
    [CustomEditor(typeof(OwnershipTransfer))]
    public class OwnershipTransferEditor : UdonLibraryEditor
    {
        protected override string GetSymbolName()
        {
            return "ownershipTransfer";
        }
    }
}
#endif
