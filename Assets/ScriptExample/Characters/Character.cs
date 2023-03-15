using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.Characters
{
    public class Character : MonoBehaviourInjectable
    {
        [Component]
        private Skill attackSkill;
        public Skill AttackSkill => attackSkill;

        [FromContainer]
        private ITestInterface testInterface;
        public ITestInterface TestInterface => testInterface;
    }
}