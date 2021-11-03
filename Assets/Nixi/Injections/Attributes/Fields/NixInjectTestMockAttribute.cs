using Nixi.Injections.Attributes.Abstractions;
using System;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Used to flag a non-component field in a class derived from MonoBehaviourInjectable, it won't fill in play mode
    /// <para/>In play mode, the field should be filled from SerializedField or some logic in class
    /// <para/>In test mode, make the field mockable with TestInjecter.InjectMock 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectTestMockAttribute : NixInjectBaseAttribute
    {
    }
}