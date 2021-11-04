using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInject : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public Skill WrongAttributeSkill;
    }
}