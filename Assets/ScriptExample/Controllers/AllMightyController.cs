using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController")]
        public SorcererController AllMightySorcererController;

        [NixInjectRootComponent("MonsterController")]
        public MonsterController AllMightyMonsterController;
    }
}