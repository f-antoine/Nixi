using Nixi.Injections.Attributes.Abstractions;
using System.Reflection;

namespace Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions
{
    /// <summary>
    /// Base attribute to represent a dependency injection on a component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>It handles single component/interface field
    /// </summary>
    public abstract class NixInjectSingleComponentBaseAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Check if attribute decorate the right field.FieldType and setup data from component field into the nixi attribute component decorator
        /// <para/>This one prevents from decorating fields that aren't interfaces and non-components
        /// </summary>
        /// <param name="field">Field info to check</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            if (field.FieldType.IsGenericType)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {field.Name} and type {field.FieldType.Name}, because it is a generic type while using decorator {GetType().Name}, " +
                                                 $"it is only possible with NixInjectComponentsAttribute on an IEnumerable, List or array");
            }

            if (!IsComponent(field.FieldType) && !field.FieldType.IsInterface)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {field.Name} and type {field.FieldType.Name}, because it not a component or an interface while using decorator {GetType().Name}");
            }
        }
    }
}