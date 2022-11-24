using System;

namespace Nixi.Containers
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of NixiContainer
    /// </summary>
    [Serializable]
    public sealed class NixiContainerException : Exception
    {
        internal NixiContainerException() { }

        internal NixiContainerException(string reason)
            : base(reason)
        {
        }
    }
}