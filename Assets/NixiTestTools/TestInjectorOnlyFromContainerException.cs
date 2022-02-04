using Nixi.Injections;
using System;

namespace NixiTestTools
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of TestInjectorOnlyFromContainerException
    /// </summary>
    [Serializable]
    public sealed class TestInjectorOnlyFromContainerException : Exception
    {
        internal TestInjectorOnlyFromContainerException(string reason, OnlyFromContainerInjectable injectable)
             : base($"Error during nixi test injection resolution on GameObject named {injectable} (with type {injectable.GetType().Name})." +
                    $"{Environment.NewLine}Reason : {reason}")
        {
        }
    }
}