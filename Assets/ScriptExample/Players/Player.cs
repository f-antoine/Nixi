using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Players
{
    public class Player : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SorcererGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;

        [NixInjectFromContainer]
        public ILifeBar LifeBar;

        [SerializeField]
        [NixInjectTestMock]
        private SO_Player soPlayerInfos;
        public SO_Player SOPlayerInfos => soPlayerInfos;
    }
}