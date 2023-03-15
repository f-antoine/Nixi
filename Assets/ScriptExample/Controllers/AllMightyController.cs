using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyController : MonoBehaviourInjectable
    {
        [RootComponent("SorcererController")]
        public SorcererController AllMightySorcererController;

        [RootComponent("MonsterController")]
        public MonsterController AllMightyMonsterController;
    }
}