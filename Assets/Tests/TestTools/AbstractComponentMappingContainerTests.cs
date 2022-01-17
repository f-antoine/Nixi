using NixiTestTools.TestInjectorElements.Utils;
using NUnit.Framework;
using ScriptExample.AbstractMapping;
using System;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class AbstractComponentMappingContainerTests
    {
        [Test]
        public void Map_ShouldThrowException_WhenAlreadyRegistered()
        {
            AbstractComponentMappingContainer container = new AbstractComponentMappingContainer();
            container.Map<AbstractComponentBase, ImplementationFromAbstract>();
            Assert.Throws<AbstractComponentMappingException>(() => container.Map<AbstractComponentBase, ImplementationFromAbstract>());
        }

        [Test]
        public void Map_ShouldThrowException_WhenTransformIsPassedAsAbstractType()
        {
            AbstractComponentMappingContainer container = new AbstractComponentMappingContainer();
            Assert.Throws<AbstractComponentMappingException>(() => container.Map<Transform, RectTransform>());
        }

        [Test]
        public void TryResolve_ShouldReturnNull_WhenNeverRegistered()
        {
            AbstractComponentMappingContainer container = new AbstractComponentMappingContainer();
            Assert.Null(container.TryResolve<AbstractComponentBase>());
            Assert.Null(container.TryResolve(typeof(AbstractComponentBase)));
        }

        [Test]
        public void MapAndResolve_ShouldReturnCorrectImplementation()
        {
            AbstractComponentMappingContainer container = new AbstractComponentMappingContainer();
            container.Map<AbstractComponentBase, ImplementationFromAbstract>();

            Type typeResolved = container.TryResolve<AbstractComponentBase>();
            Assert.AreEqual(typeof(ImplementationFromAbstract).Name, typeResolved.Name);

            Type typeResolvedWithTypeArgument = container.TryResolve(typeof(AbstractComponentBase));
            Assert.AreEqual(typeof(ImplementationFromAbstract).Name, typeResolvedWithTypeArgument.Name);
        }
    }
}