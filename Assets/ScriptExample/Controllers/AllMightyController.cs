using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyController : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("SorcererController")]
        public SorcererController AllMightySorcererController;

        [NixInjectMonoBehaviourFromMethodRoot("MonsterController")]
        public MonsterController AllMightyMonsterController;
    }
}