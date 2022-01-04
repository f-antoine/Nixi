using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.Characters
{
    public class Character : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private Skill attackSkill;
        public Skill AttackSkill => attackSkill;

        [NixInjectFromContainer]
        private ITestInterface testInterface;
        public ITestInterface TestInterface => testInterface;
    }
}