using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.SingleComponent
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component attached on current GameObject with GetComponents
    /// and get the one that matches the type
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="ComponentFromParentsAttribute">Use ComponentFromParentsAttribute to handle parents only (excluding current gameObject)</see>
    /// <para/><see cref="ComponentFromChildrenAttribute">Use ComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ComponentAttribute : NixInjectSingleComponentBaseAttribute
    {
        /// <summary>
        /// Finds the component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method and parameters previously registered
        /// <para/>This one is used to retrieve all the UnityEngine.Component attached on current GameObject with GetComponents
        /// and get the one that matches the type
        /// </summary>
        /// <returns>Unique component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method</returns>
        protected override object GetComponentResultFromParameters()
        {
            Component[] componentsFound = Target.GetComponents(FieldType);
            return CheckAndGetSingleComponentAtInjectableLevel(componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponentAtInjectableLevel(Component[] componentsFound)
        {
            if (!componentsFound.Any())
            {
                throw new NixiAttributeException($"No component with type {FieldType.Name} was found to fill field with name " +
                                                 $"{FieldName}", FieldType, FieldName);
            }

            if (componentsFound.Length > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {FieldType.Name} to fill field " +
                                                 $"with name {FieldName}, could not define which one should be used " +
                                                 $"({componentsFound.Length} found instead of just one, please use NixInjectComponentsAttribute)", FieldType, FieldName);
            }

            return componentsFound.Single();
        }
    }

    #region Obsolete version
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component attached on current GameObject with GetComponents
    /// and get the one that matches the type
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parents only (excluding current gameObject)</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    [Obsolete("Will be replaced with a shorter version : Component")]
    public sealed class NixInjectComponentAttribute : ComponentAttribute { }
    #endregion Obsolete version
}