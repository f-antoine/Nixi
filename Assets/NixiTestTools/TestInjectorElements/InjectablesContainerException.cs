using System;

namespace NixiTestTools.TestInjectorElements
{
    /// <summary>
    /// Errors reported when there is an exception thrown during usage of InjectablesContainer in TestInjector
    /// </summary>
    [Serializable]
    internal sealed class InjectablesContainerException : Exception
    {
        internal InjectablesContainerException() { }

        internal InjectablesContainerException(string reason)
            : base(reason)
        {
        }
    }
}