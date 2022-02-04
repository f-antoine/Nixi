using NixiTestTools;
using NUnit.Framework;
using ScriptExample.OnlyFromContainer;
using System;

namespace Tests.TestTools.InjectableOnlyFromContainerTestTemplateCases
{
    internal sealed class InjectableOnlyFromContainerTestTemplate_ForcedConstructorTests : InjectableOnlyFromContainerTestTemplate<AllInjectorCases>
    {
        protected override Func<AllInjectorCases> MainTestedConstructionMethod => () => new AllInjectorCases(36, false);

        [Test]
        public void ForceConstructorMethod_InTestTemplate_ShouldForceThisConstructorToBeUsed()
        {
            // MainTestedConstructionMethod
            Assert.NotNull(MainTested.TestInterface);
            Assert.AreEqual(36, MainTested.TestInterface.ValueToRetrieve);

            // Force to use this method
            ForceMainTestedConstructionMethod = () => new AllInjectorCases(48, false);

            // Invoke forced method
            ResetTemplate();

            // ForceMainTestedConstructionMethod
            Assert.NotNull(MainTested.TestInterface);
            Assert.AreEqual(48, MainTested.TestInterface.ValueToRetrieve);

            // Null value means it will use MainTestedConstructionMethod
            ForceMainTestedConstructionMethod = null;

            // Invoke first method again : MainTestedConstructionMethod
            ResetTemplate();

            Assert.NotNull(MainTested.TestInterface);
            Assert.AreEqual(36, MainTested.TestInterface.ValueToRetrieve);
        }
    }
}