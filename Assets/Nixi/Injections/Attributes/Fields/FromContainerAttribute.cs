using Nixi.Injections.Attributes.Fields.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes.Fields
{
    /// <summary>
    /// Used to trigger the injection with a classic dependency injection from NixiContainer in an interface field
    /// in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses container to fill it
    /// <para/>In tests, make the field mockable with TestInjector.InjectField 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FromContainerAttribute : NixInjectBaseAttribute
    {
        /// <summary>
        /// Check if the field decorated by this attribute (derived from NixInjectAbstractBaseAttribute) is valid and fill it
        /// <para/>This one prevents from decorating fields that aren't interface
        /// </summary>
        /// <param name="field">Field info to check</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            if (typeof(Component).IsAssignableFrom(field.FieldType))
            {
                throw new NixiAttributeException($"Cannot register field with name {field.Name} with a NixInjectAttribute because " +
                                                 $"it is a Component field, you must use NixInjectComponentAttribute instead", field.FieldType, field.Name);
            }

            if (!field.FieldType.IsInterface)
            {
                throw new NixiAttributeException($"The field with the name {field.Name} with a NixInjectAttribute must be an interface " +
                                                 $"because the container works only with interfaces as a key for injection, " +
                                                 $"if you don't want to use the container and only expose for the tests from template, " +
                                                 $"you can use NixInjectTestMockAttribute", field.FieldType, field.Name);
            }
        }
    }

    #region Obsolete version
    /// <summary>
    /// Used to trigger the injection with a classic dependency injection from NixiContainer in an interface field
    /// in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses container to fill it
    /// <para/>In tests, make the field mockable with TestInjector.InjectField 
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    [Obsolete("Will be replaced with a shorter version : FromContainer")]
    public sealed class NixInjectFromContainerAttribute : FromContainerAttribute { }
    #endregion Obsolete version
}