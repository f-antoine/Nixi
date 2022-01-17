using System;
using System.Reflection;

namespace Nixi.Injections.Abstractions
{
    /// <summary>
    /// Common abstract base attribute for all NixInjections
    /// <para/>All attributes derived from this base attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// </summary>
    public abstract class NixInjectAbstractBaseAttribute : Attribute
    {
        /// <summary>
        /// Check if the field decorated by this attribute (derived from NixInjectAbstractBaseAttribute) is valid and fill it
        /// </summary>
        /// <param name="field">Field info to check</param>
        public virtual void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            // Do nothing by default
        }
    }
}