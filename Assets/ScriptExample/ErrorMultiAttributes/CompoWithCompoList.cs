﻿using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoList : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponents]
        public Sorcerer Sorcerer;
    }
}