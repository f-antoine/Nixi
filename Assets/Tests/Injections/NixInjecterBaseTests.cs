using Assets.Tests.DataSets;
using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injecters;
using NUnit.Framework;
using ScriptExample.Containers;
using Tests.Builders;

namespace Assets.Tests.Injections
{
    internal sealed class NixInjecterBaseTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
        }

        [Test]
        public void InjectTwice_ShouldThrowException()
        {
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithChildSkill().BuildNixInjecter();

            nixInjecter.CheckAndInjectAll();

            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [TestCaseSource(typeof(AllErrorMultiAttributes))]
        public void InjectFieldWithMultiple_ShouldThrowException(MonoBehaviourInjectable monoBehaviourInjectable)
        {
            NixInjecter injecter = new NixInjecter(monoBehaviourInjectable);
            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }
    }
}