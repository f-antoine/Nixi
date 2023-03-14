using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyControllerFull : MonoBehaviourInjectable
    {
        [ComponentFromParents("TheFirstChildSorcererController")]
        public SorcererController FirstChildSorcererController;

        [ComponentFromChildren("TheSecondChildSorcererController")]
        public SorcererController SecondChildSorcererController;

        [RootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}