using Nixi.Injections;
using ScriptExample.Characters;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Players
{
    public class Player : MonoBehaviourInjectable
    {
        [ComponentFromChildren("SorcererGameObjectName")]
        public Sorcerer Sorcerer;

        [FromContainer]
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