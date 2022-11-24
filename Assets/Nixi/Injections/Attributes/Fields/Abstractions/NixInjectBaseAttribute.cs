using Nixi.Injections.Attributes.Abstractions;
using System;

namespace Nixi.Injections.Attributes.Fields.Abstractions
{
    /// <summary>
    /// Base attribute thats represent a classic dependency injection from NixiContainer
    /// <para/>All attributes derived from this base attribute must be used on a non-component field
    /// in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses NixiContainer to fill it
    /// <para/>In tests, make the field mockable with TestInjector.InjectField 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectBaseAttribute : NixInjectAbstractBaseAttribute
    {
    }
}