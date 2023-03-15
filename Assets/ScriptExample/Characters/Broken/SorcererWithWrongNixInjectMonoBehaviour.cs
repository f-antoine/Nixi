using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectComponent : MonoBehaviourInjectable
    {
        [Component]
        public ScriptableObject brokenTestSecond;
    }
}