using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes.Abstractions
{
    /// <summary>
    /// Common abstract base attribute for all NixInjections
    /// </summary>
    public abstract class NixInjectAbstractBaseAttribute : Attribute
    {
        /// <summary>
        /// Check if a type is a Component
        /// </summary>
        /// <param name="typeToCheck">Type to check</param>
        /// <returns>True if this is a component</exception>
        protected static bool IsComponent(Type typeToCheck)
        {
            return typeof(Component).IsAssignableFrom(typeToCheck);
        }

        /// <summary>
        /// Check if attribute decorate the right field.FieldType and setup data from component field into the nixi attribute component decorator
        /// </summary>
        /// <param name="field">Field info to check</param>
        public virtual void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            // Do nothing by default
        }
    }
}