using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyControllerFull : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("TheFirstChildSorcererController", GameObjectMethod.GetComponentsInParent)]
        public SorcererController FirstChildSorcererController;

        [NixInjectMonoBehaviourFromMethod("TheSecondChildSorcererController", GameObjectMethod.GetComponentsInChildren)]
        public SorcererController SecondChildSorcererController;

        [NixInjectMonoBehaviourFromMethodRoot("MonsterController")]
        public MonsterController MonsterController;
    }
}