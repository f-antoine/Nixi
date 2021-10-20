using Nixi.Injections;
using Nixi.Injections.Attributes;
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

        [NixInjectFromContainer]
        private ITestInterface testInterface;
        public ITestInterface TestInterface => testInterface;
    }
}