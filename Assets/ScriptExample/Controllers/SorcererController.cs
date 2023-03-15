using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [RootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}