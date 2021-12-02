using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

namespace Guribo.UdonUtils.Runtime.Common.Networking
{
    /// <summary>
    /// Component which is used to synchronize a value on demand independently from high continuous synced udon
    /// behaviours to reduce bandwidth.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedInteger : UdonSharpBehaviour
    {
        /// <summary>
        /// resets the value back to 0 after it was successfully sent
        /// </summary>
        public bool autoResetOnSuccess;
        
        [UdonSynced, FieldChangeCallback(nameof(IntValueProperty))]
        internal int SyncedValue;

        public string[] targetFieldNames;
        public UdonSharpBehaviour[] listeners;

        public int IntValueProperty
        {
            set
            {
                var valueUnchanged = SyncedValue == value;
                if (valueUnchanged)
                {
                    return;
                }

                SyncedValue = value;

                if (VRC.SDKBase.Networking.IsOwner(gameObject))
                {
                    RequestSerialization();
                }

                NotifyListeners();
            }
            get { return SyncedValue; }
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            if (result.success)
            {
                if (autoResetOnSuccess)
                {
                    IntValueProperty = 0;
                }
                
                return;
            }
            
            SendCustomEventDelayedSeconds(nameof(RequestSerialization), 1f);
        }

        internal void NotifyListeners()
        {
            var listenersInvalid = listeners == null
                                   || targetFieldNames == null
                                   || listeners.Length != targetFieldNames.Length;
            if (listenersInvalid)
            {
                Debug.LogError("Invalid listener setup");
                return;
            }

            for (var i = 0; i < listeners.Length; i++)
            {
                if (Utilities.IsValid(listeners[i]))
                {
                    listeners[i].SetProgramVariable(targetFieldNames[i], SyncedValue);
                }
            }
        }
    }
}