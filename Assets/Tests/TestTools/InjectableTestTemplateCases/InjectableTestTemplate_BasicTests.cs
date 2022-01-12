using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;

namespace Tests.TestTools.InjectableTestTemplateCases
{
    internal sealed class InjectableTestTemplate_BasicTests : InjectableTestTemplate<Sorcerer>
    {
        [Test]
        public void Constructor_Should_LoadMainTestedAndMainInjecter()
        {
            Assert.NotNull(MainTested);
            Assert.NotNull(MainInjecter);
            Assert.NotNull(MainTested.AttackSkill); // Check injected
            int instanceId = MainTested.GetInstanceID();
            int injecterHash = MainInjecter.GetHashCode();
            int attackSkillId = MainTested.AttackSkill.GetInstanceID();

            // Refill with other values
            ResetTemplate();

            Assert.AreNotEqual(instanceId, MainTested.GetInstanceID());
            Assert.AreNotEqual(injecterHash, MainInjecter.GetHashCode());
            Assert.AreNotEqual(attackSkillId, MainTested.AttackSkill.GetInstanceID()); // Check re-injected
        }
    }
}