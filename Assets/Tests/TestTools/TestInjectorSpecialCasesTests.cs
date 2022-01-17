using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Geometrics;
using ScriptExample.Geometrics.Inheritance;
using ScriptExample.RootComponents;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorSpecialCasesTests
    {
        [Test]
        public void Transform_ShouldBeInstantiated_OnCurrentLevel()
        {
            TransformSquare square = InjectableBuilder<TransformSquare>.Create().Build();
            TestInjector Injector = new TestInjector(square);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(square.Transform);
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.Transform.GetInstanceID()));

            // Check from Injector
            Transform transformFromInjector = Injector.GetComponent<Transform>();
            Assert.That(transformFromInjector.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
        }

        [Test]
        public void DoubleTransform_ShouldBeInstantiated_WithSameTransform_OnCurrentLevel()
        {
            TwoSameTransformSquare square = InjectableBuilder<TwoSameTransformSquare>.Create().Build();
            TestInjector Injector = new TestInjector(square);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(square.Transform);
            Assert.NotNull(square.SecondTransform);
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.Transform.GetInstanceID()));
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.SecondTransform.GetInstanceID()));

            // Check from Injector
            Transform transformFromInjector = Injector.GetComponent<Transform>("Transform");
            Transform secondTransformFromInjector = Injector.GetComponent<Transform>("SecondTransform");
            Assert.That(transformFromInjector.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
            Assert.That(secondTransformFromInjector.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
        }

        [Test]
        public void RectTransform_ShouldBeInstantiated_OnCurrentLevel()
        {
            RectTransformSquare rectSquare = InjectableBuilder<RectTransformSquare>.Create().Build();
            TestInjector Injector = new TestInjector(rectSquare);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(rectSquare.RectTransform);
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.transform.GetInstanceID()));

            // Check from Injector
            RectTransform transformFromInjector = Injector.GetComponent<RectTransform>();
            Assert.That(transformFromInjector.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
        }

        [Test]
        public void DoubleRectTransform_ShouldBeInstantiated_WithSameTransform_OnCurrentLevel()
        {
            TwoSameRectTransformSquare rectSquare = InjectableBuilder<TwoSameRectTransformSquare>.Create().Build();
            TestInjector Injector = new TestInjector(rectSquare);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(rectSquare.RectTransform);
            Assert.NotNull(rectSquare.SecondRectTransform);
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.transform.GetInstanceID()));
            Assert.That(rectSquare.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.gameObject.GetInstanceID()));
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.SecondRectTransform.transform.GetInstanceID()));
            Assert.That(rectSquare.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.SecondRectTransform.gameObject.GetInstanceID()));

            // Check from Injector
            RectTransform transformFromInjector = Injector.GetComponent<RectTransform>("RectTransform");
            RectTransform secondTransformFromInjector = Injector.GetComponent<RectTransform>("SecondRectTransform");
            Assert.That(transformFromInjector.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
            Assert.That(transformFromInjector.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.gameObject.GetInstanceID()));
            Assert.That(secondTransformFromInjector.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
            Assert.That(secondTransformFromInjector.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.gameObject.GetInstanceID()));
        }

        [Test]
        public void DifferentTransform_ShouldThrowException_WhenInjected()
        {
            TwoDifferentTransformSquare differentSquare = InjectableBuilder<TwoDifferentTransformSquare>.Create().Build();
            TestInjector Injector = new TestInjector(differentSquare);

            Assert.Throws<TestInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void RootComponent_MustExcludeRootItSelfOnGetComponentInChildren()
        {
            RootWithExcludingItself element = InjectableBuilder<RootWithExcludingItself>.Create().Build();
            TestInjector Injector = new TestInjector(element, "CurrentLevel");
            Injector.CheckAndInjectAll();

            Assert.AreNotEqual(element.CurrentImage.GetInstanceID(), element.ChildImage.GetInstanceID());
            Assert.AreNotEqual(element.CurrentImage.gameObject.GetInstanceID(), element.ChildImage.gameObject.GetInstanceID());
        }

        [Test]
        public void TestInjectorShouldNotLog_InheritedRectTransformSquareWithImage_ShouldInstantiateRectTransform_WithoutInformationMessage()
        {
            // True if log was written
            bool cantAddRectTransformLogGotCalled = false;

            // Should not create log : Can't add component 'RectTransform' to New Game Object because such a component is already added to the game object!
            Application.logMessageReceived += (condition, stackTrace, logType) =>
            {
                cantAddRectTransformLogGotCalled = true;
            };

            // Act
            InheritedRectTransformSquareWithImage button = new GameObject().AddComponent<InheritedRectTransformSquareWithImage>();
            TestInjector Injector = new TestInjector(button);
            Injector.CheckAndInjectAll();

            // Check
            Assert.False(cantAddRectTransformLogGotCalled);
        }
    }
}