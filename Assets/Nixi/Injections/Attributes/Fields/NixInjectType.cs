namespace Nixi.Injections.Attributes.Fields
{
    /// <summary>
    /// Type of injection wanted
    /// </summary>
    public enum NixInjectType
    {
        /// <summary>
        /// Default behaviour, will automatically use the NixiContainer to fill the field during play mode, and can be mockable during tests
        /// </summary>
        FillWithContainer = 0,

        /// <summary>
        /// Will not use the NixiContainer to fill automatically the field during play mode, but can be mockable during tests
        /// <para/> This can be useful if you want to use SerializeField
        /// </summary>
        DoesNotFillButExposeForTesting = 1
    }
}