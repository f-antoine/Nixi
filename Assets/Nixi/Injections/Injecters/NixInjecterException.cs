using System;

namespace Nixi.Injections.Injecters
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of NixInjecter
    /// </summary>
    [Serializable]
    public sealed class NixInjecterException : Exception
    {
        internal NixInjecterException() { }

        internal NixInjecterException(string reason, MonoBehaviourInjectable monoBehaviourInjectable)
            : base($"Error during nixi injection resolution on {monoBehaviourInjectable.name}. Reason : {reason}")
        {
        }
    }
}