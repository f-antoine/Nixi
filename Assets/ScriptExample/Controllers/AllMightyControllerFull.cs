using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyControllerFull : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("TheFirstChildSorcererController", GameObjectMethod.GetComponentsInParent)]
        public SorcererController FirstChildSorcererController;

        [NixInjectComponentFromMethod("TheSecondChildSorcererController", GameObjectMethod.GetComponentsInChildren)]
        public SorcererController SecondChildSorcererController;

        [NixInjectRootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}