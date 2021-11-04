namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// State returned by InjectableHandler when linking or creating a new Component
    /// </summary>
    public enum InjectableComponentState
    {
        /// <summary>
        /// TestInjecter has nothing more to call, because everything were just linked
        /// </summary>
        NoNeedToInject,

        /// <summary>
        /// TestInjecter has to call InjectAndStoreIfIsMonoBehaviourInjectable because a new component was added or created and need to be injected if type is injectable
        /// </summary>
        NeedToBeInjectedIfInjectable
    }
}