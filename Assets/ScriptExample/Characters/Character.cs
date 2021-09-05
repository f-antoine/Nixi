using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Containers;
using UnityEngine;

namespace ScriptExample.Characters
{
    public class Character : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectMonoBehaviour]
        private Skill attackSkill;
        public Skill AttackSkill => attackSkill;

        [NixInject]
        private ITestInterface testInterface;
        public ITestInterface TestInterface => testInterface;
    }
}