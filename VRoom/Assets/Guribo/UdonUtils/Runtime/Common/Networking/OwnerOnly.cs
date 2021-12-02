using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Guribo.UdonUtils.Runtime.Common.Networking
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class OwnerOnly : UdonSharpBehaviour
    {
        public GameObject[] gameObjects;
        public UdonSharpBehaviour[] UdonSharpBehaviours;

        public void OnEnable()
        {
            ToggleEnabled(VRC.SDKBase.Networking.IsOwner(gameObject));
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            ToggleEnabled(VRC.SDKBase.Networking.IsOwner(gameObject));
        }

        private void ToggleEnabled(bool enable)
        {
            if (gameObjects == null)
            {
                return;
            }

            foreach (var go in gameObjects)
            {
                if (!Utilities.IsValid(go))
                {
                    continue;
                }

                if (go.activeSelf == enable)
                {
                    continue;
                }
                
                go.SetActive(enable);                
            }

            if (UdonSharpBehaviours == null)
            {
                return;
            }
            
            foreach (var udonSharpBehaviour in UdonSharpBehaviours)
            {
                if (!Utilities.IsValid(udonSharpBehaviour))
                {
                    continue;
                }

                udonSharpBehaviour.enabled = enable;
            }
        }
    }
}