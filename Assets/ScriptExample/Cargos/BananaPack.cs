using Nixi.Injections;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.Cargos
{
    public sealed class BananaPack : MonoBehaviourInjectable
    {
        [RootComponent("MasterLight")]
        private Light bananeLighter;
        public Light BananeLighter => bananeLighter;

        [ComponentFromChildren("rainbowGeneratorName")]
        private ParticleSystem rainbowGenerator;
        public ParticleSystem RainbowGenerator => rainbowGenerator;

        [RootComponent("Rainbow Master")]
        private Skill rainbowSummonSkill;
        public Skill RainbowSummonSkill => rainbowSummonSkill;

        [RootComponent("Rainbow Master", "RainbowFuel")]
        private Skill rainbowFuelSkill;
        public Skill RainbowFuelSkill => rainbowFuelSkill;

        public int NbBanane;
    }
}