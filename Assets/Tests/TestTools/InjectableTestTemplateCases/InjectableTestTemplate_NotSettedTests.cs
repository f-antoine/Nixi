using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;

namespace Tests.TestTools.InjectableTestTemplateCases
{
    internal sealed class InjectableTestTemplate_NotSettedTests : InjectableTestTemplate<Sorcerer>
    {
        protected override bool SetTemplateWithConstructor => false;

        [Test]
        public void Constructor_ShouldNot_LoadMainTestedAndMainInjector()
        {
            Assert.Null(MainTested);
            Assert.Null(MainInjector);

            // Fill for the first time
            ResetTemplate();

            Assert.NotNull(MainTested);
            Assert.NotNull(MainInjector);
            Assert.NotNull(MainTested.AttackSkill); // Check injected
        }
    }
}