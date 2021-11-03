﻿using Nixi.Injections.Attributes;
using ScriptExample.Characters;
using UnityEngine;

namespace Assets.ScriptExample.Characters
{
    public sealed class Warrior : Character
    {
        [SerializeField]
        [NixInjectTestMock]
        public Parasite Parasite;
    }
}