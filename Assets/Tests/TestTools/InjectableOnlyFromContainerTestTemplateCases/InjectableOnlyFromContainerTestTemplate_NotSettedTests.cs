using NixiTestTools;
using NUnit.Framework;
using ScriptExample.OnlyFromContainer.TestInjector;
using System;

namespace Tests.TestTools.InjectableOnlyFromContainerTestTemplateCases
{
    internal sealed class InjectableOnlyFromContainerTestTemplate_NotSettedTests : InjectableOnlyFromContainerTestTemplate<AllInjectorCases>
    {
        protected override bool SetTemplateWithConstructor => false;

        protected override Func<AllInjectorCases> MainTestedConstructionMethod => () => new AllInjectorCases(false);

        [Test]
        public void Constructor_ShouldNot_LoadMainTestedAndMainInjector()
        {
            Assert.Null(MainTested);
            Assert.Null(MainInjector);

            // Fill for the first time
            ResetTemplate();

            Assert.NotNull(MainTested);
            Assert.NotNull(MainInjector);
        }
    }
}