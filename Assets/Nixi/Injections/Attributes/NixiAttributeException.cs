using System;

namespace Nixi.Injections
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of nixi attribute methods
    /// </summary>
    [Serializable]
    public sealed class NixiAttributeException : Exception
    {
        internal NixiAttributeException() { }

        internal NixiAttributeException(string reason)
            : base(reason)
        {
        }
    }
}