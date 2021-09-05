using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Players
{
    public class Player : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("SorcererGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;

        [NixInject]
        public ILifeBar LifeBar;

        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)]
        [SerializeField]
        private SO_Player soPlayerInfos;
        public SO_Player SOPlayerInfos => soPlayerInfos;
    }
}