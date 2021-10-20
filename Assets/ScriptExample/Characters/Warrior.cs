using Nixi.Injections.Attributes.Fields;
using ScriptExample.Characters;
using UnityEngine;

namespace Assets.ScriptExample.Characters
{
    public sealed class Warrior : Character
    {
        [SerializeField]
        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)]
        public Parasite Parasite;
    }
}