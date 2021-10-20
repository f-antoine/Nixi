using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters.ScriptableObjects;
using UnityEngine;

namespace ScriptExample.Characters
{
    public sealed class Sorcerer : Character
    {
        [SerializeField]
        [NixInjectComponentFromMethod("SorcererChildGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        private Skill magicSkill;
        public Skill MagicSkill => magicSkill;
        
        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)][SerializeField]
        private SO_Sorcerer soInfos;
        public SO_Sorcerer SOInfos => soInfos;

        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)][SerializeField]
        private SO_InventoryBag firstInventoryBagInfos;
        public SO_InventoryBag FirstInventoryBagInfos => firstInventoryBagInfos;

        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)][SerializeField]
        private SO_InventoryBag secondInventoryBagInfos;
        public SO_InventoryBag SecondInventoryBagInfos => secondInventoryBagInfos;

    }
}