using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Characters;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Players
{
    public class Player : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("SorcererGameObjectName")]
        public Sorcerer Sorcerer;

        [NixInjectFromContainer]
        public ILifeBar LifeBar;

        [SerializeField]
        private SO_Player soPlayerInfos;
        public SO_Player SOPlayerInfos => soPlayerInfos;

        protected override void Awake()
        {
            base.Awake();
        }
    }
}