using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;

namespace Assets.ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectNotInterface : MonoBehaviourInjectable
    {
        [NixInject]
        public string WrongAttributeNotInterface;
    }
}