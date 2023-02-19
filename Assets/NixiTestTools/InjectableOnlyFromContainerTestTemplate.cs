using Nixi.Injections;
using System;

namespace NixiTestTools
{
    // TODO : Tests
    // TODO : Refacto
    // TODO : Comments
    // TODO : How to pass data if constructor require a call on a NixInjectFromContainer component in TestInjector ?
    // TODO : Pass to v1.1.0 (dll metadata + unity file)
    // TODO : Rename MainTestedConstructionMethod / ForceMainTestedConstructionMethod, not easy to use when MainTested is written
    // TODO : Comment
    // TODO : Update script on documentation for protected set MainTested / MainInjector
    public abstract class InjectableOnlyFromContainerTestTemplate<T>
        where T : OnlyFromContainerInjectable
    {
        // TODO : Comment
        protected abstract Func<T> MainTestedConstructionMethod { get; }
        protected Func<T> ForceMainTestedConstructionMethod = null;

        // TODO : Comment
        protected T MainTested { get; set; }

        // TODO : Comment
        protected TestInjectorOnlyFromContainer MainInjector { get; set; }

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