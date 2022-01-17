using System;

namespace Nixi.Injections.Injectors
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of NixInjector
    /// </summary>
    [Serializable]
    public sealed class NixInjectorException : Exception
    {
        internal NixInjectorException() { }

        internal NixInjectorException(string reason, MonoBehaviourInjectable injectable)
            : base($"Error during nixi injection resolution on GameObject named {injectable.name} (with type {injectable.GetType().Name})." +
                   $"{Environment.NewLine}Reason : {reason}")
        {
        }
    }
}