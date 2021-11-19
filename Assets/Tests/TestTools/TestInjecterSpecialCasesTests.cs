using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Geometrics;
using ScriptExample.Geometrics.Inheritance;
using ScriptExample.RootComponents;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterSpecialCasesTests
    {
        [Test]
        public void Transform_ShouldBeInstantiated_OnCurrentLevel()
        {
            TransformSquare square = InjectableBuilder<TransformSquare>.Create().Build();
            TestInjecter injecter = new TestInjecter(square);
            injecter.CheckAndInjectAll();

            // Check
            Assert.NotNull(square.Transform);
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.Transform.GetInstanceID()));

            // Check from injecter
            Transform transformFromInjecter = injecter.GetComponent<Transform>();
            Assert.That(transformFromInjecter.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
        }

        [Test]
        public void DoubleTransform_ShouldBeInstantiated_WithSameTransform_OnCurrentLevel()
        {
            TwoSameTransformSquare square = InjectableBuilder<TwoSameTransformSquare>.Create().Build();
            TestInjecter injecter = new TestInjecter(square);
            injecter.CheckAndInjectAll();

            // Check
            Assert.NotNull(square.Transform);
            Assert.NotNull(square.SecondTransform);
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.Transform.GetInstanceID()));
            Assert.That(square.transform.GetInstanceID(), Is.EqualTo(square.SecondTransform.GetInstanceID()));

            // Check from injecter
            Transform transformFromInjecter = injecter.GetComponent<Transform>("Transform");
            Transform secondTransformFromInjecter = injecter.GetComponent<Transform>("SecondTransform");
            Assert.That(transformFromInjecter.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
            Assert.That(secondTransformFromInjecter.GetInstanceID(), Is.EqualTo(square.transform.GetInstanceID()));
        }

        [Test]
        public void RectTransform_ShouldBeInstantiated_OnCurrentLevel()
        {
            RectTransformSquare rectSquare = InjectableBuilder<RectTransformSquare>.Create().Build();
            TestInjecter injecter = new TestInjecter(rectSquare);
            injecter.CheckAndInjectAll();

            // Check
            Assert.NotNull(rectSquare.RectTransform);
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.transform.GetInstanceID()));

            // Check from injecter
            RectTransform transformFromInjecter = injecter.GetComponent<RectTransform>();
            Assert.That(transformFromInjecter.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
        }

        [Test]
        public void DoubleRectTransform_ShouldBeInstantiated_WithSameTransform_OnCurrentLevel()
        {
            TwoSameRectTransformSquare rectSquare = InjectableBuilder<TwoSameRectTransformSquare>.Create().Build();
            TestInjecter injecter = new TestInjecter(rectSquare);
            injecter.CheckAndInjectAll();

            // Check
            Assert.NotNull(rectSquare.RectTransform);
            Assert.NotNull(rectSquare.SecondRectTransform);
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.transform.GetInstanceID()));
            Assert.That(rectSquare.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.RectTransform.gameObject.GetInstanceID()));
            Assert.That(rectSquare.transform.GetInstanceID(), Is.EqualTo(rectSquare.SecondRectTransform.transform.GetInstanceID()));
            Assert.That(rectSquare.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.SecondRectTransform.gameObject.GetInstanceID()));

            // Check from injecter
            RectTransform transformFromInjecter = injecter.GetComponent<RectTransform>("RectTransform");
            RectTransform secondTransformFromInjecter = injecter.GetComponent<RectTransform>("SecondRectTransform");
            Assert.That(transformFromInjecter.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
            Assert.That(transformFromInjecter.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.gameObject.GetInstanceID()));
            Assert.That(secondTransformFromInjecter.GetInstanceID(), Is.EqualTo(rectSquare.transform.GetInstanceID()));
            Assert.That(secondTransformFromInjecter.gameObject.GetInstanceID(), Is.EqualTo(rectSquare.gameObject.GetInstanceID()));
        }

        [Test]
        public void DifferentTransform_ShouldThrowException_WhenInjected()
        {
            TwoDifferentTransformSquare differentSquare = InjectableBuilder<TwoDifferentTransformSquare>.Create().Build();
            TestInjecter injecter = new TestInjecter(differentSquare);

            Assert.Throws<TestInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [Test]
        public void RootComponent_MustExcludeRootItSelfOnGetComponentInChildren()
        {
            RootWithExcludingItself element = InjectableBuilder<RootWithExcludingItself>.Create().Build();
            TestInjecter injecter = new TestInjecter(element, "CurrentLevel");
            injecter.CheckAndInjectAll();

            Assert.AreNotEqual(element.CurrentImage.GetInstanceID(), element.ChildImage.GetInstanceID());
            Assert.AreNotEqual(element.CurrentImage.gameObject.GetInstanceID(), element.ChildImage.gameObject.GetInstanceID());
        }

        [Test]
        public void TestInjecterShouldNotLog_InheritedRectTransformSquareWithImage_ShouldInstantiateRectTransform_WithoutInformationMessage()
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
            TestInjecter injecter = new TestInjecter(button);
            injecter.CheckAndInjectAll();

            // Check
            Assert.False(cantAddRectTransformLogGotCalled);
        }
    }
}