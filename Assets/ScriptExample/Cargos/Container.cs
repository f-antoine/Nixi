using Nixi.Injections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ScriptExample.Cargos
{
    public sealed class Container : MonoBehaviourInjectable
    {
        [Components]
        private List<BananaPack> firstBananaPacks = new List<BananaPack>();
        public List<BananaPack> FirstBananaPacks => firstBananaPacks;

        [Components]
        private List<BananaPack> secondBananaPacks = new List<BananaPack>();
        public List<BananaPack> SecondBananaPacks => secondBananaPacks;

        [ComponentsFromParents]
        private List<BananaPack> parentBananaPacks = new List<BananaPack>();
        public List<BananaPack> ParentBananaPacks => parentBananaPacks;

        [ComponentsFromChildren]
        private List<BananaPack> childBananaPacks = new List<BananaPack>();
        public List<BananaPack> ChildBananaPacks => childBananaPacks;

        [Component]
        private Button openCloseButton;
        public Button OpenCloseButton => openCloseButton;

        [ComponentFromChildren("logoImg")]
        private Image logoImg;
        public Image LogoImg => logoImg;
    }
}
