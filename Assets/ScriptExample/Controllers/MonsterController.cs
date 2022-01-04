using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class MonsterController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController")]
        public SorcererController SorcererController;
    }
}