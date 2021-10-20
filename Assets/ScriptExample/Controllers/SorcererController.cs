using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}