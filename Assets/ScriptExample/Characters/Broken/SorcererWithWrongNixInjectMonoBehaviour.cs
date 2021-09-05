using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Containers.Broken;

namespace Assets.ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectMonoBehaviour : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviour]
        public IBrokenTestInterface brokenTestInterfaceSecond;
    }
}