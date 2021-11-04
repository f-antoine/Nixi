using Nixi.Injections.Attributes.Abstractions;
using System;
using System.Reflection;

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
        /// <summary>
        /// Check if attribute decorate the right field.FieldType and setup data from component field into the nixi attribute component decorator
        /// <para/>This one prevents from decorating fields that aren't interface
        /// </summary>
        /// <param name="field">Field info to check</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            if (IsComponent(field.FieldType))
                throw new NixiAttributeException($"Cannot register field with name {field.Name} with a NixInjectAttribute because it is a Component field, you must use NixInjectComponentAttribute instead");

            if (!field.FieldType.IsInterface)
                throw new NixiAttributeException($"The field with the name {field.Name} with a NixInjectAttribute must be an interface " +
                    $"because the container works only with interfaces as a key for injection, " +
                    $"if you don't want to use the container and only expose for the tests from template, " +
                    $"you can use NixInjectTestMockAttribute");
        }
    }
}