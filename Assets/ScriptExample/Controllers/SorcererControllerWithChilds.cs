using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChilds ChildMonsterController;
    }
}