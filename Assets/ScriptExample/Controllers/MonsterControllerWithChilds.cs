using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChilds ChildSorcererController;
    }
}