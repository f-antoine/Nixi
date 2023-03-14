using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInjectNotInterface : MonoBehaviourInjectable
    {
        [FromContainer]
        public string WrongAttributeNotInterface;
    }
}