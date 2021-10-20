using System.Collections.Generic;

namespace NixiTestTools.TestInjecterElements.Relations.Abstractions
{
    /// <summary>
    /// Contains all relations between components of type T (parent/child chain) and handle all FieldInfo injections of theses relations
    /// </summary>
    /// <typeparam name="T">Type handled in link used in Relations</typeparam>
    /// <typeparam name="FieldHandled">Type of field handle for FieldInfo injections</typeparam>
    /// <see cref="Relation{T}"/>
    internal abstract class RelationHandlerBase<T, FieldHandled> : FieldHandler<FieldHandled>
        where T : class
        where FieldHandled : SimpleFieldInfo
    {
        #region Fields handling
        /// <summary>
        /// Add a field to the list of fields of type FieldHandled
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal virtual void AddFieldAndLink(FieldHandled fieldToAdd)
        {
            AddField(fieldToAdd);
        }
        #endregion Fields handling

        #region Relations handling
        /// <summary>
        /// Contains all relations (links) of type T
        /// </summary>
        protected List<Relation<T>> Relations = new List<Relation<T>>();

        /// <summary>
        /// Add a relation to the list of Relations
        /// </summary>
        /// <param name="parent">Parent element of the relation</param>
        /// <param name="childs">Child list of element inheriting (linked) from the parent</param>
        internal virtual void AddRelation(T parent, List<Relation<T>> childs = null)
        {
            Relations.Add(new Relation<T>
            {
                Parent = parent,
                Childs = childs ?? new List<Relation<T>>()
            });
        }
        #endregion Relations handling
    }
}
