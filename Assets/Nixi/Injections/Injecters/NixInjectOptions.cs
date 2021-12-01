namespace Nixi.Injections.Injecters
{
    public sealed class NixInjectOptions
    {
        /// <summary>
        /// Set to true if you want to use SerializeField on field decorated with Nixi attribute during PlayMode.
        /// <para/> This help to show how it is fill in play mode, but cannot be used in tests
        /// <para/> Default is false, it means you cannot decorated a field with Nixi attribute and SerializeField
        /// </summary>
        public bool AuthorizeSerializedFieldWithNixiAttributes = false;
    }
}