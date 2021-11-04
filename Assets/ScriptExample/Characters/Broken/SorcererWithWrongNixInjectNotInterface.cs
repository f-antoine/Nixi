using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectNotInterface : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public string WrongAttributeNotInterface;
    }
}