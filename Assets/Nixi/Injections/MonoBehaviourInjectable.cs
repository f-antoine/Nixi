using Nixi.Injections.Injectors;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// MonoBehaviour derived class created to use Nixi dependency injection and Unity dependency injection on fields decorated with Nixi attributes
    /// <list type="table">
    ///     <item>
    ///         <term>Play mode scene usage</term>
    ///         <description>NixInjector passed in BuildInjections</description>
    ///     </item>
    ///     <item>
    ///         <list type="bullet">
    ///             <item>
    ///                 <term>Component fields</term>
    ///                 <description>Marked with attributes derived from NixInjectComponentBaseAttribute will be populated with Unity dependency injection</description>
    ///             </item>
    ///             <item>
    ///                 <term>Non-Component fields</term>
    ///                 <description>Marked with attributes derived from NixInjectBaseAttribute will be populated with NixiContainer</description>
    ///             </item>
    ///         </list>
    ///     </item>
    ///     
    ///     <item>
    ///         <term>Tests usage (via InjectionTestTemplate)</term>
    ///         <description>TestInjector passed in BuildInjections</description>
    ///     </item>
    ///     <item>
    ///         <list type="bullet">
    ///             <item>
    ///                 <term>Component fields</term>
    ///                 <description>Marked with attributes derived from NixInjectComponentBaseAttribute will be created, 
    ///                 used to populate the field and registered in TestInjector property of InjectionTestTemplate</description>
    ///             </item>
    ///             <item>
    ///                 <term>Non-Component fields</term>
    ///                 <description>Marked with attributes derived from NixInjectBaseAttribute will be mockable, you can inject mock in it</description>
    ///             </item>
    ///         </list>
    ///     </item>
    /// </list>
    /// </summary>
    public abstract class MonoBehaviourInjectable : MonoBehaviour
    {
        /// <summary>
        /// Options available to parameterized the injections, default 
        /// </summary>
        protected virtual NixInjectOptions NixInjectOptions => null;

        /// <summary>
        /// Awake method from Unity will do the injection in Unity play mode scene
        /// </summary>
        protected virtual void Awake()
        {
            new NixInjector(this, NixInjectOptions).CheckAndInjectAll();
        }
    }
}