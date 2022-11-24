using NixiTestTools;
using NUnit.Framework;
using ScriptExample.OnlyFromContainer.TestInjector;
using System;

namespace Tests.TestTools.InjectableOnlyFromContainerTestTemplateCases
{
    internal sealed class InjectableOnlyFromContainerTestTemplate_BasicTests : InjectableOnlyFromContainerTestTemplate<AllInjectorCases>
    {
        protected override Func<AllInjectorCases> MainTestedConstructionMethod => () => new AllInjectorCases(false);

        [Test]
        public void Constructor_Should_LoadMainTestedAndMainInjector()
        {
            Assert.NotNull(MainTested);
            Assert.NotNull(MainInjector);
            int mainTestedHash = MainTested.GetHashCode();
            int injectorHash = MainInjector.GetHashCode();

            // Refill with other values
            ResetTemplate();

            Assert.AreNotEqual(mainTestedHash, MainTested.GetHashCode());
            Assert.AreNotEqual(injectorHash, MainInjector.GetHashCode());
        }
    }
}