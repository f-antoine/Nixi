using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class MonsterController : MonoBehaviourInjectable
    {
        [RootComponent("SorcererController")]
        public SorcererController SorcererController;
    }
}