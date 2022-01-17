using Nixi.Containers;
using ScriptExample.Containers.Player;
using UnityEngine;

namespace ScriptExample.Containers
{
    /// <summary>
    /// Create all the mappings between interface and implementation used in NixiContainer
    /// </summary>
    public static class InjectionConfiguration
    {
        /// <summary>
        /// Create all the mappings between interfaces and implementations used in NixiContainer before splash screen and only once
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void LinkFieldInjections()
        {
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            NixiContainer.MapSingleton<ILifeBar, LifeBar>();
        }
    }
}