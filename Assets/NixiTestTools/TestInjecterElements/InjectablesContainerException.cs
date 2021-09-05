using System;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of InjectablesContainer in TestInjecter
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