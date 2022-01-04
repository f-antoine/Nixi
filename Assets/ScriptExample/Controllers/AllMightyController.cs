using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController")]
        public SorcererController AllMightySorcererController;

        [NixInjectRootComponent("MonsterController")]
        public MonsterController AllMightyMonsterController;
    }
}