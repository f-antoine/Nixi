using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
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