using System;

namespace NixiTestTools.TestInjectorElements.Utils
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of AbstractComponentMappings/AbstractComponentMappingContainer
    /// </summary>
    [Serializable]
    public sealed class AbstractComponentMappingException : Exception
    {
        internal AbstractComponentMappingException() { }

        internal AbstractComponentMappingException(string reason)
            : base(reason)
        {
        }
    }
}