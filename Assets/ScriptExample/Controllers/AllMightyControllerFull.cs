using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyControllerFull : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("TheFirstChildSorcererController")]
        public SorcererController FirstChildSorcererController;

        [NixInjectComponentFromChildren("TheSecondChildSorcererController")]
        public SorcererController SecondChildSorcererController;

        [NixInjectRootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}