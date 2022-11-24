using Nixi.Injections.Injectors;

namespace Nixi.Injections
{
    // TODO : Update here
    // TODO : Comment
    public abstract class OnlyFromContainerInjectable
    {
        // TODO : Comment
        // TODO : Readonly if ok
        private NixInjectorOnlyFromContainer injector;

        public OnlyFromContainerInjectable(bool autoInject)
        {
            if (autoInject)
            {
                injector = injector ?? new NixInjectorOnlyFromContainer(this);
                injector.CheckAndInjectAll();
            }
        }
    }
}