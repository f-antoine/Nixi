using Nixi.Injections;
using System;

namespace NixiTestTools
{
    // TODO : Pass to v1.1.0 (dll metadata + unity file)
    // TODO : Comment
    public abstract class InjectableOnlyFromContainerTestTemplate<T>
        where T : OnlyFromContainerInjectable
    {
        // TODO : Comment
        protected abstract Func<T> MainTestedConstructionMethod { get; }
        protected Func<T> ForceMainTestedConstructionMethod = null;

        // TODO : Comment
        protected T MainTested { get; private set; }

        // TODO : Comment
        protected TestInjectorOnlyFromContainer MainInjector { get; private set; }

        // TODO : Comment
        protected virtual bool SetTemplateWithConstructor => true;

        // TODO : Comment
        protected virtual void ResetTemplate()
        {
            BuildMainTested();

            MainInjector = new TestInjectorOnlyFromContainer(MainTested);

            MainInjector.CheckAndInjectAll();
        }

        // TODO : Comment
        private void BuildMainTested()
        {
            if (ForceMainTestedConstructionMethod == null)
            {
                MainTested = MainTestedConstructionMethod();
            }
            else
            {
                MainTested = ForceMainTestedConstructionMethod();
            }
        }

        // TODO : Comment
        public InjectableOnlyFromContainerTestTemplate()
        {
            if (SetTemplateWithConstructor)
                ResetTemplate();
        }
    }
}