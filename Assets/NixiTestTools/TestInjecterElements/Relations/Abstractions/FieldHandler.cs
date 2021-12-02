using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Name to use in exception thrown to be more precise about the type of field targeted
        /// </summary>
        protected virtual string fieldTypeName => "field";

        /// <summary>
        /// List of all FieldInfos encapsulated (of type FieldHandled)
        /// </summary>
        internal IReadOnlyList<FieldHandled> Fields => fields.AsReadOnly();

        /// <summary>
        /// Add a field to the list of fields of type FieldHandled
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal virtual void AddField(FieldHandled fieldToAdd)
        {
            fields.Add(fieldToAdd);
        }

        /// <summary>
        /// Method to use in Where condition on fields to match typeToFind
        /// </summary>
        /// <param name="typeToFind">FieldType of FieldInfo to find</param>
        /// <returns>Single FieldHandled with type equals to typeToFind</returns>
        protected virtual Func<FieldHandled, bool> FieldMatchType(Type typeToFind)
        {
            return x => x.FieldInfo.FieldType == typeToFind;
        }

        /// <summary>
        /// Base method to get a FieldInfo with type equals to typeToFind
        /// </summary>
        /// <param name="typeToFind">FieldType of FieldInfo to find</param>
        /// <returns>Instance of class with targeted FieldInfo attached</returns>
        internal FieldHandled GetFieldInfoHandler(Type typeToFind)
        {
            IEnumerable<FieldHandled> fieldSelecteds = fields.Where(FieldMatchType(typeToFind));

            if (!fieldSelecteds.Any())
                throw new InjectablesContainerException($"no {fieldTypeName} with type {typeToFind.Name} was found");

            if (fieldSelecteds.Count() > 1)
                throw new InjectablesContainerException($"multiple {fieldTypeName}s with type {typeToFind.Name} were found, cannot define which one use, please use 'fieldName' in method input parameters instead");

            return fieldSelecteds.Single();
        }

        /// <summary>
        /// Base method to get a FieldInfo with type equals to typeToFind and with name fieldName
        /// </summary>
        /// <param name="typeToFind">FieldType of FieldInfo to find</param>
        /// <param name="fieldName">FieldName of FieldInfo to Find</param>
        /// <returns>nstance of class with targeted FieldInfo attached</returns>
        internal FieldHandled GetFieldInfoHandler(Type typeToFind, string fieldName)
        {
            IEnumerable<FieldHandled> fieldsWithType = fields.Where(FieldMatchType(typeToFind));

            if (!fieldsWithType.Any())
                throw new InjectablesContainerException($"no {fieldTypeName} with type {typeToFind.Name} was found");

            IEnumerable<FieldHandled> fieldsWithTypeAndName = fieldsWithType.Where(x => x.FieldInfo.Name == fieldName);

            if (!fieldsWithTypeAndName.Any())
                throw new InjectablesContainerException($"{fieldTypeName} with type {typeToFind.Name} was/were found, but none with fieldName {fieldName}");

            return fieldsWithTypeAndName.Single();
        }
    }
}