﻿using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections;

namespace Assets.ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckRootCompo : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("anyRootName")]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}