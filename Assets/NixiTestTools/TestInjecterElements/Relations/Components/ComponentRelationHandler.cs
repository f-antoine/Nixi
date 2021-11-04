using Nixi.Injections.Attributes;
using Nixi.Injections.Attributes.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.Components
{
    /// <summary>
    /// Contains all relations between components of type Component (parent/child chain) and handle all FieldInfo injections of theses relations
    /// </summary>
    internal sealed class ComponentRelationHandler : RelationHandlerBase<Component, ComponentWithFieldInfo>
    {
        /// <summary>
        /// Add a field to the list of fields of type ComponentWithFieldInfo and add a relation with his component at top parent if this is NixInjectComponentAttribute
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal override void AddFieldAndLink(ComponentWithFieldInfo fieldToAdd)
        {            
            if (fieldToAdd.FieldInfo.GetCustomAttribute<NixInjectComponentAttribute>() != null)
                AddRelation(fieldToAdd.Component);

            base.AddFieldAndLink(fieldToAdd);
        }

        /// <summary>
        /// Return the only component with type searched at first level of the relations list (NixInjectComponent => GetComponent in Unity dependency injection way)
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