using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Abstractions
{
    /// <summary>
    /// Common abstract base attribute for all NixInjections
    /// </summary>
    public abstract class NixInjectAbstractBaseAttribute : Attribute
    {
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