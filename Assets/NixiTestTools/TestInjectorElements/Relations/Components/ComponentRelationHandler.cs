using NixiTestTools.TestInjectorElements.Relations.Abstractions;
using System;
using System.Linq;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.Components
{
    /// <summary>
    /// Contains all relations between components (parent/children links) and keep an access to all their FieldInfo (wrapped into ComponentWithFieldInfo)
    /// to handle all injections of theses relations
    /// </summary>
    internal sealed class ComponentRelationHandler : RelationHandlerBase<Component, ComponentWithFieldInfo>
    {
        /// <summary>
        /// Name to use in exception thrown to be more precise about the type of field targeted
        /// </summary>
        protected override string FieldTypeName => "component";

        /// <summary>
        /// Return the only component with type searched at first level of the relations list
        /// <para/>Simulate : NixInjectComponent => GetComponent in Unity dependency injection way
        /// </summary>
        /// <param name="typeToFind">Type of component searched</param>
        /// <returns>Component that match typeToFind and on the first level of the relations list</returns>
        internal Component GetSameLevelComponent(Type typeToFind)
        {
            Relation<Component> relationFound = Relations.SingleOrDefault(x => x.Parent.GetType() == typeToFind);
            return relationFound?.Parent;
        }
    }
}