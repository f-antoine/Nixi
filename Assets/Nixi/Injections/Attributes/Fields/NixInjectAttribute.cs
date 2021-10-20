using System;

namespace Nixi.Injections.Attributes.Fields
{
    /// <summary>
    /// Used to trigger the injection in a Non-Component field in a class derived from MonoBehaviourInjectable
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectAttribute : Attribute
    {
        /// <summary>
        /// Type of injection wanted, default is : FillWithContainer
        /// </summary>
        public NixInjectType NixInjectType { get; private set; }

        /// <summary>
        /// Used to trigger the injection in a Non-Component field in a class derived from MonoBehaviourInjectable
        /// By default this will fill the decorated field with the NixiContainer
        /// <see cref="Fields.NixInjectType"/>
        /// </summary>
        /// <param name="nixInjectType">Type of injection wanted</param>
        public NixInjectAttribute(NixInjectType nixInjectType = NixInjectType.FillWithContainer)
        {
            NixInjectType = nixInjectType;
        }
    }
}