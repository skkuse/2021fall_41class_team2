using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Guribo.UdonUtils.Runtime.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class UdonDebug : UdonSharpBehaviour
    {
        /// <summary>
        /// Notes: Has no effect unless the corresponding compiler flag is set in Unity via the menu
        /// <b>Guribo/UdonUtils/Log Assertion Errors/Enable</b>.<br/>
        /// The option is saved in the Editor Preferences and will be the same across multiple Unity projects that use
        /// the same Unity version!
        ///<br/><br/>
        /// When active it will return false if the condition is false, it will log an error message filled
        /// with the name of the context object the error occurred on and with the given error message.
        /// </summary>
        /// <param name="condition">Condition expected to be true, false with log the error message</param>
        /// <param name="message">Compact error message, will be surrounded by context info</param>
        /// <param name="context">Object which is relevant to the condition failing, usually a behaviour or gameobject</param>
        /// <returns>The value of condition</returns>
        public bool Assert(bool condition, string message, Object context)
        {
#if !GURIBO_DEBUG
            return condition;
#else
            if (!condition)
            {
                if (Utilities.IsValid(context))
                {
                    var udonSharpBehaviour = (UdonSharpBehaviour) context;
                    if (Utilities.IsValid(udonSharpBehaviour))
                    {
                        Debug.LogError($"Assertion failed : '{udonSharpBehaviour.gameObject.name} : {message}'", context);
                    }
                    else
                    {
                        Debug.LogError($"Assertion failed : '{context.GetType()} : {message}'", context);
                    }
                }
                else
                {
                    Debug.LogError("Assertion failed :  '" + message + "'");
                }

                return false;
            }

            Debug.Assert(condition, message);
            return true;
#endif
        }
    }
}