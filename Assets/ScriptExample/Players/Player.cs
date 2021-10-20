using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Players
{
    public class Player : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SorcererGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;

        [NixInject]
        public ILifeBar LifeBar;

        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)]
        [SerializeField]
        private SO_Player soPlayerInfos;
        public SO_Player SOPlayerInfos => soPlayerInfos;
    }
}