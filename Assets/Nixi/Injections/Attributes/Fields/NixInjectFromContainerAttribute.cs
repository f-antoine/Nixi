using System;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Used to trigger the injection in a non-component field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode, it uses container to fill it
    /// <para/>In test mode, make the field mockable with TestInjecter.InjectMock 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectFromContainerAttribute : NixInjectBaseAttribute
    {
    }
}