using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Characters;

namespace Assets.ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInject : MonoBehaviourInjectable
    {
        [NixInject]
        public Skill WrongAttributeSkill;
    }
}