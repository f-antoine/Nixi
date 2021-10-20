﻿using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections;

namespace Assets.ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckRootCompoWithChildGameObject : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("anyRootName", "anyChildGameObjectName")]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}
