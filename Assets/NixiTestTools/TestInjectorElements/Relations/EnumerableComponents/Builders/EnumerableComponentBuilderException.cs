using System;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Builders
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of EnumerableComponentBuilder
    /// </summary>
    [Serializable]
    public sealed class EnumerableComponentBuilderException : Exception
    {
        internal EnumerableComponentBuilderException() { }

        internal EnumerableComponentBuilderException(string reason)
            : base(reason)
        {
        }
    }
}