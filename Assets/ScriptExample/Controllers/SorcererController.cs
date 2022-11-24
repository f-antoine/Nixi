using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}