using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.Cargos
{
    public sealed class BananaPack : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MasterLight")]
        private Light bananeLighter;
        public Light BananeLighter => bananeLighter;

        [NixInjectComponentFromChildren("rainbowGeneratorName")]
        private ParticleSystem rainbowGenerator;
        public ParticleSystem RainbowGenerator => rainbowGenerator;

        [NixInjectRootComponent("Rainbow Master")]
        private Skill rainbowSummonSkill;
        public Skill RainbowSummonSkill => rainbowSummonSkill;

        [NixInjectRootComponent("Rainbow Master", "RainbowFuel")]
        private Skill rainbowFuelSkill;
        public Skill RainbowFuelSkill => rainbowFuelSkill;

        public int NbBanane;
    }
}