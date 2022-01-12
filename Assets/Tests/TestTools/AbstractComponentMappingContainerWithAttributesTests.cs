using NixiTestTools;
using NUnit.Framework;
using ScriptExample.AbstractMapping;
using System.Collections.Generic;

namespace Tests.TestTools
{
    internal sealed class AbstractComponentMappingContainerWithAttributesTests : InjectableTestTemplate<ExamplesWithAbstractComponent>
    {
        protected override bool SetTemplateWithConstructor => false;

        [SetUp]
        public void InitTests()
        {
            AddAbstractComponentMapping<AbstractComponentBase, ImplementationFromAbstract>();
            ResetTemplate();
        }

        [Test]
        public void ImplementationType_MustBe_InstantiatedOnAbstractComponents()
        {
            List<AbstractComponentBase> baseComponents = new List<AbstractComponentBase>
            {
                MainTested.InstanceTransposed,
                MainTested.InstanceTransposedChildren,
                MainTested.InstanceTransposedChildrenWithName,
                MainTested.InstanceTransposedParent,
                MainTested.InstanceTransposedParentWithName,
                MainTested.RootInstanceTransposed,
                MainTested.RootChildrenInstanceTransposed,
            };

            string implementationFromAbstractName = typeof(ImplementationFromAbstract).Name;
            foreach (var element in baseComponents)
            {
                Assert.NotNull(element);
                Assert.AreEqual(implementationFromAbstractName, element.GetType().Name);
                Assert.AreEqual(123, element.ValueToRetrieve);
            }
        }
    }
}