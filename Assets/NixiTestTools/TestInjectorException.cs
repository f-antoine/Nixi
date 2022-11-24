using Nixi.Injections;
using System;

namespace NixiTestTools
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of TestInjector
    /// </summary>
    [Serializable]
    public sealed class TestInjectorException : Exception
    {
        internal TestInjectorException(string reason, MonoBehaviourInjectable injectable)
            : base($"Error during nixi test injection resolution on GameObject named {injectable.name} (with type {injectable.GetType().Name})." +
                   $"{Environment.NewLine}Reason : {reason}")
        {
        }
    }
}