using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [RootComponent("MonsterController")]
        public MonsterController MonsterController;
    }
}