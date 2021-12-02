using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Guribo.UdonUtils.Runtime.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class ReflectionProbeController : UdonSharpBehaviour
    {
        #region Libraries

        [Header("Libraries")]
        public UdonDebug udonDebug;

        #endregion

        public ReflectionProbe reflectionProbe;

        [Range(0, 60)]
        public float updateInterval = 10f;

        public void OnEnable()
        {
            if (!udonDebug.Assert(Utilities.IsValid(reflectionProbe), "reflectionProbe invalid", this))
            {
                return;
            }

            UpdateReflections();
        }

        public void UpdateReflections()
        {
            if (!(gameObject.activeInHierarchy
                  && enabled
                  && udonDebug.Assert(Utilities.IsValid(reflectionProbe), "reflectionProbe invalid", this)))
            {
                return;
            }

            reflectionProbe.RenderProbe();

            SendCustomEventDelayedSeconds(nameof(UpdateReflections), updateInterval);
        }
    }
}