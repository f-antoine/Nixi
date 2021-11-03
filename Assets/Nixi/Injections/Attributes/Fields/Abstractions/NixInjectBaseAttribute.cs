using System;

namespace Nixi.Injections.Attributes.Abstractions
{
    /// <summary>
    /// Base attribute to derive, used to trigger the injection in a non-component field in a class derived from MonoBehaviourInjectable
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectBaseAttribute : NixInjectAbstractBaseAttribute
    {
    }
}