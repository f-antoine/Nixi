using Assets.Scripts.Injections;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class SubMonoBehaviourInjectable : MonoBehaviourInjectable
    {
        [SerializeField]
        [ComponentInject("CatInjection")]
        private Cat SubNeedToBeInjected;
    }
}