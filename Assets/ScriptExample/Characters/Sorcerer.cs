using Nixi.Injections.Attributes;
using ScriptExample.Characters.ScriptableObjects;
using UnityEngine;

namespace ScriptExample.Characters
{
    public sealed class Sorcerer : Character
    {
        [NixInjectComponentFromChildren("SorcererChildGameObjectName")]
        private Skill magicSkill;
        public Skill MagicSkill => magicSkill;

        [SerializeField]
        private SO_Sorcerer soInfos;
        public SO_Sorcerer SOInfos => soInfos;

        [SerializeField]
        private SO_InventoryBag firstInventoryBagInfos;
        public SO_InventoryBag FirstInventoryBagInfos => firstInventoryBagInfos;

        [SerializeField]
        private SO_InventoryBag secondInventoryBagInfos;
        public SO_InventoryBag SecondInventoryBagInfos => secondInventoryBagInfos;
    }
}