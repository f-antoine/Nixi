using System;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Errors reported when there is an exception thrown during the usage of nixi attribute methods
    /// </summary>
    [Serializable]
    public sealed class NixiAttributeException : Exception
    {
        public string Reason { get; private set; }
        public Type TargetType { get; private set; }
        public string TargetName { get; private set; }

        public NixiAttributeException() { }

        // TODO : Check if start every message with "cannot inject field..." ? With the Injector Name ?
        public NixiAttributeException(string reason, Type targetType, string targetName)
            : base($"{reason}. TargetType {targetType.Name}, TargetName : {targetName}")
        {
            Reason = Message;
            TargetType = targetType;
            TargetName = targetName;
        }
    }
}