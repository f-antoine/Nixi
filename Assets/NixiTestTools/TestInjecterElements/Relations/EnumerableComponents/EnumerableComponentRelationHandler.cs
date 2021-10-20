using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Components;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.EnumerableComponents
{
    /// <summary>
    /// Contains all relations between components of type "list of component" (parent/child chain) and handle all FieldInfo injections of theses relations
    /// </summary>
    internal sealed class EnumerableComponentRelationHandler : RelationHandlerBase<IEnumerable<Component>, ComponentListWithFieldInfo>
    {
        /// <summary>
        /// Add a field to the list of fields of type ComponentWithFieldInfo and add a relation with his component at top parent
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal override void AddFieldAndLink(ComponentListWithFieldInfo fieldToAdd)
        {
            AddRelation(fieldToAdd.Components);
            base.AddFieldAndLink(fieldToAdd);
        }
    }
}