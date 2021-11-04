using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ScriptExample.Cargos
{
    public sealed class Container : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<BananaPack> firstBananaPacks = new List<BananaPack>();
        public List<BananaPack> FirstBananaPacks => firstBananaPacks;

        [NixInjectComponents]
        private List<BananaPack> secondBananaPacks = new List<BananaPack>();
        public List<BananaPack> SecondBananaPacks => secondBananaPacks;

        [NixInjectComponentsFromParent]
        private List<BananaPack> parentBananaPacks = new List<BananaPack>();
        public List<BananaPack> ParentBananaPacks => parentBananaPacks;

        [NixInjectComponentsFromChildren]
        private List<BananaPack> childBananaPacks = new List<BananaPack>();
        public List<BananaPack> ChildBananaPacks => childBananaPacks;

        [NixInjectComponent]
        private Button openCloseButton;
        public Button OpenCloseButton => openCloseButton;

        [NixInjectComponentFromChildren("logoImg")]
        private Image logoImg;
        public Image LogoImg => logoImg;
    }
}
