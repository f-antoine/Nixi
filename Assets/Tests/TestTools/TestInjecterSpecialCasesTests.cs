using ScriptExample.Geometrics;
using NixiTestTools;
using NUnit.Framework;
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
    }
}