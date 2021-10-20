using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Containers.Broken;

namespace Assets.ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IBrokenTestInterface brokenTestInterfaceSecond;
    }
}