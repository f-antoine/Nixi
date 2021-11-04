﻿using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class RectTransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public RectTransform RectTransform;
    }
}