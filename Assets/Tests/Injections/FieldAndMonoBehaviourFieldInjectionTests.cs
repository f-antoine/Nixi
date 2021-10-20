using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Containers;
using ScriptExample.FieldTests;

namespace Tests.Injections
{
    internal sealed class FieldAndcomponentFieldInjectionTests : InjectableTestTemplate<ChildFields>
    {
        [TestCase("privateParentTestInterface")]
        [TestCase("protectedParentTestInterface")]
        [TestCase("publicParentTestInterface")]
        [TestCase("privateChildTestInterface")]
        [TestCase("protectedChildTestInterface")]
        [TestCase("publicChildTestInterface")]
        public void FieldInjection_ShouldLoadMockInAllFields_WhatEverVisibilityLevel(string fieldName)
        {
            Assert.That(MainTested.GetInterfaceFieldFromName(fieldName), Is.Null);

            // Arrange
            Mock<ITestInterface> fieldMock = new Mock<ITestInterface>(MockBehavior.Strict);
            fieldMock.SetupGet(x => x.ValueToRetrieve).Returns(1).Verifiable();

            // Act
            MainInjecter.InjectMock(fieldMock.Object, fieldName);

            // Assert
            Assert.That(MainTested.GetInterfaceFieldFromName(fieldName), Is.Not.Null);
            Assert.That(MainTested.GetInterfaceFieldFromName(fieldName).ValueToRetrieve, Is.EqualTo(1));
            fieldMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [TestCase("privateParentSkill")]
        [TestCase("protectedParentSkill")]
        [TestCase("publicParentSkill")]
        [TestCase("privateChildSkill")]
        [TestCase("protectedChildSkill")]
        [TestCase("publicChildSkill")]
        public void ComponentFieldInjection_ShouldBeFilledAndAccessible_WhatEverVisibilityLevel(string fieldName)
        {
            Skill skillToFind = MainInjecter.GetComponent<Skill>(fieldName);
            Assert.That(skillToFind, Is.Not.Null);
            Assert.That(skillToFind.name, Is.EqualTo(fieldName));
        }
    }
}
