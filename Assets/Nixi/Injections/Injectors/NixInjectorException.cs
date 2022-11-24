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

        internal NixInjectorException(string reason, string injectableName, Type injectableType)
            : base($"Error during nixi injection resolution on injectable with name {injectableName} and with type {injectableType.Name}." +
                   $"{Environment.NewLine}Reason : {reason}")
        {
        }
    }
}