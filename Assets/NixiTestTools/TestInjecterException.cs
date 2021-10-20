using Nixi.Injections;
using System;

namespace NixiTestTools
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of TestInjecter
    /// </summary>
    [Serializable]
    public sealed class TestInjecterException : Exception
    {
        internal TestInjecterException(string reason, MonoBehaviourInjectable monoBehaviourInjectable)
            : base($"Error during nixi test injection resolution on {monoBehaviourInjectable.name} (type {monoBehaviourInjectable.GetType().Name}). Reason : {reason}")
        {
        }
    }
}