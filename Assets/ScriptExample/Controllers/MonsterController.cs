using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class MonsterController : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("SorcererController")]
        public SorcererController SorcererController;
    }
}