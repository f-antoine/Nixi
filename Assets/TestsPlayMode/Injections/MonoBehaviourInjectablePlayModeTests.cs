using Nixi.Containers;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Containers;
using UnityEngine;

namespace TestsPlayMode.Injections
{
    internal sealed class MonoBehaviourInjectablePlayModeTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();

            TestImplementation implementation = new TestImplementation
            {
                ValueToRetrieve = 1234
            };
            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(implementation);
        }

        [Test]
        public void Awake_ShouldInject()
        {
            // Prepare
            GameObject injectableGameObject = new GameObject();
            
            Skill skill = injectableGameObject.AddComponent<Skill>();

            // Add Character call Awake on it because it is in "Play Mode tests" and populate fields with injections
            Character character = injectableGameObject.AddComponent<Character>();

            // Checks
            Assert.AreEqual(skill.GetInstanceID(), character.AttackSkill.GetInstanceID());
            Assert.AreEqual(1234, character.TestInterface.ValueToRetrieve);
        }
    }
}