using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.ScriptExample.Cargos
{
    public sealed class Container : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        private List<BananaPack> firstBananaPacks = new List<BananaPack>();
        public List<BananaPack> FirstBananaPacks => firstBananaPacks;

        [NixInjectComponentList]
        private List<BananaPack> secondBananaPacks = new List<BananaPack>();
        public List<BananaPack> SecondBananaPacks => secondBananaPacks;

        [NixInjectComponent]
        private Button openCloseButton;
        public Button OpenCloseButton => openCloseButton;

        [NixInjectComponentFromMethod("logoImg", GameObjectMethod.GetComponentsInChildren)]
        private Image logoImg;
        public Image LogoImg => logoImg;
    }
}
