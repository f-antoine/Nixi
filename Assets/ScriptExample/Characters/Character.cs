using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Containers;
using UnityEngine;

namespace ScriptExample.Characters
{
    public class Character : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponent]
        private Skill attackSkill;
        public Skill AttackSkill => attackSkill;

        [NixInject]
        private ITestInterface testInterface;
        public ITestInterface TestInterface => testInterface;
    }
}