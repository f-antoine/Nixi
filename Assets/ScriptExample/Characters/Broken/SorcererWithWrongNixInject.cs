using Nixi.Injections;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class SorcererWithWrongNixInject : MonoBehaviourInjectable
    {
        [FromContainer]
        public Skill WrongAttributeSkill;
    }
}