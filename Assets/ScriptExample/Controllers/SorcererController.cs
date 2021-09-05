using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class SorcererController : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("MonsterController")]
        public MonsterController MonsterController;
    }
}