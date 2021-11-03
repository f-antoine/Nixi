using System.Collections.Generic;

namespace NixiTestTools.TestInjecterElements.Relations.Abstractions
{
    /// <summary>
    /// Contains all fields of type FieldHandled (fieldInfo derivation)
    /// </summary>
    /// <typeparam name="FieldHandled">Type of field handle for FieldInfo injections</typeparam>
    internal class FieldHandler<FieldHandled>
        where FieldHandled : SimpleFieldInfo
    {
        /// <summary>
        /// List of all FieldInfos encapsulated (of type FieldHandled)
        /// </summary>
        private List<FieldHandled> fields = new List<FieldHandled>();

        /// <summary>
        /// List of all FieldInfos encapsulated (of type FieldHandled)
        /// </summary>
        internal IReadOnlyList<FieldHandled> Fields => fields.AsReadOnly();

        /// <summary>
        /// Add a field to the list of fields of type FieldHandled
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        public virtual void AddField(FieldHandled fieldToAdd)
        {
            fields.Add(fieldToAdd);
        }
    }
}