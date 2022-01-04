using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}