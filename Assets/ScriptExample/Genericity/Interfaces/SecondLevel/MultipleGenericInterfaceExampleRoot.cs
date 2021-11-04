﻿using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName")]
        public IMultipleGenericInterface<int, string> RootGenericityInterface;
    }
}