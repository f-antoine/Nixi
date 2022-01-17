using NixiTestTools.TestInjectorElements.Relations.Fields;
using System.Collections.Generic;

namespace NixiTestTools.TestInjectorElements.Relations.Abstractions
{
    /// <summary>
    /// Contains all relations between a class of type T (parent/children links) and keep an access to all their FieldInfo (wrapped into FieldHandled)
    /// to handle all injections of theses relations
    /// </summary>
    /// <typeparam name="T">Type handled in link used in Relations</typeparam>
    /// <typeparam name="FieldHandled">Type of FieldInfo wrapper on which value injections are performed</typeparam>
    /// <see cref="Relation{T}"/>
    internal abstract class RelationHandlerBase<T, FieldHandled> : FieldHandler<FieldHandled>
        where T : class
        where FieldHandled : SimpleFieldInfo
    {
        #region Relations handling
        /// <summary>
        /// Contains all relations (links) of type T
        /// </summary>
        protected List<Relation<T>> Relations = new List<Relation<T>>();

        /// <summary>
        /// Add a relation to the list of Relations
        /// </summary>
        /// <param name="parent">Parent element of the relation</param>
        /// <param name="children">Child list of element inheriting (linked) from the parent</param>
        internal virtual void AddRelation(T parent, List<Relation<T>> children = null)
        {
            Relations.Add(new Relation<T>
            {
                Parent = parent,
                Children = children ?? new List<Relation<T>>()
            });
        }
        #endregion Relations handling
    }
}
