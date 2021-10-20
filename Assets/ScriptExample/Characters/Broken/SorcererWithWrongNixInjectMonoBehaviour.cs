using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace Assets.ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public ScriptableObject brokenTestSecond;
    }
}