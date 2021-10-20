using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Controllers
{
    public sealed class MonsterController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController")]
        public SorcererController SorcererController;
    }
}