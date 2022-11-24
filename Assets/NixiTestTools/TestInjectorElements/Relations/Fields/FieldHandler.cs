using System;
using System.Collections.Generic;
using System.Linq;

namespace NixiTestTools.TestInjectorElements.Relations.Fields
{
    /// <summary>
    /// Contains/Handles all fields of type that match FieldHandled
    /// </summary>
    /// <typeparam name="FieldHandled">Type of FieldInfo wrapper on which value injections are performed</typeparam>
    internal class FieldHandler<FieldHandled>
        where FieldHandled : SimpleFieldInfo
    {
        /// <summary>
        /// List of FieldHandled (type of FieldInfo wrapper on which value injections are performed)
        /// </summary>
        private readonly List<FieldHandled> fields = new List<FieldHandled>();

        /// <summary>
        /// Name to use in exception thrown to be more precise about the type of field targeted
        /// </summary>
        protected virtual string FieldTypeName => "field";

        /// <summary>
        /// ReadOnlyList of FieldHandled (type of FieldInfo wrapper on which value injections are performed)
        /// </summary>
        internal IReadOnlyList<FieldHandled> Fields => fields.AsReadOnly();

        /// <summary>
        /// Add a field into the list of FieldHandled
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal virtual void AddField(FieldHandled fieldToAdd)
        {
            fields.Add(fieldToAdd);
        }

        /// <summary>
        /// Method used to find a FieldHandled into the list of FieldHandled
        /// </summary>
        protected virtual Func<FieldHandled, Type, bool> MethodToFindFieldHandled
            => (x, typeToFind) => x.FieldInfo.FieldType == typeToFind;

        /// <summary>
        /// Base method to get single FieldInfo that match typeToFind using MethodToFindFieldHandled property
        /// </summary>
        /// <param name="typeToFind">Type to find into the list of FieldHandled</param>
        /// <returns>Unique instance of FieldHandled</returns>
        internal FieldHandled GetFieldInfoHandler(Type typeToFind)
        {
            IEnumerable<FieldHandled> fieldSelecteds = fields.Where(x => MethodToFindFieldHandled(x, typeToFind));

            if (!fieldSelecteds.Any())
            {
                throw new InjectablesContainerException($"no {FieldTypeName} with type {typeToFind.Name} was found");
            }

            if (fieldSelecteds.Count() > 1)
            {
                throw new InjectablesContainerException($"multiple {FieldTypeName}s with type {typeToFind.Name} were found, " +
                                                        $"cannot define which one use, please use 'fieldName' in method input parameters instead");
            }

            return fieldSelecteds.Single();
        }

        /// <summary>
        /// Base method to get single FieldInfo that match typeToFind and fieldName using MethodToFindFieldHandled property
        /// </summary>
        /// <param name="typeToFind">Type to find into the list of FieldHandled</param>
        /// <param name="fieldName">FieldInfo.Name to find into the list of FieldHandled</param>
        /// <returns>Unique instance of FieldHandled</returns>
        internal FieldHandled GetFieldInfoHandler(Type typeToFind, string fieldName)
        {
            IEnumerable<FieldHandled> fieldsWithType = fields.Where(x => MethodToFindFieldHandled(x, typeToFind));

            if (!fieldsWithType.Any())
            {
                throw new InjectablesContainerException($"no {FieldTypeName} with type {typeToFind.Name} was found");
            }

            IEnumerable<FieldHandled> fieldsWithTypeAndName = fieldsWithType.Where(x => x.FieldInfo.Name == fieldName);

            if (!fieldsWithTypeAndName.Any())
            {
                throw new InjectablesContainerException($"{FieldTypeName} with type {typeToFind.Name} was/were found, " +
                                                        $"but none with fieldName {fieldName}");
            }

            return fieldsWithTypeAndName.Single();
        }
    }
}