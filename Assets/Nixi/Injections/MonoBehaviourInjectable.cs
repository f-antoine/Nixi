﻿using Nixi.Injections.Injecters;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// MonoBehaviour derived class created to use Nixi dependency injection on fields decorated with Nixi attributes
    /// <list type="table">
    ///     <item>
    ///         <term>Play mode usage</term>
    ///         <description>NixInjecter passed in BuildInjections</description>
    ///     </item>
    ///     <item>
    ///         <list type="bullet">
    ///             <item>
    ///                 <term>Component fields</term>
    ///                 <description>Marked with NixInjectComponentAttribute will be populated with Unity dependency injection</description>
    ///             </item>
    ///             <item>
    ///                 <term>Non-Component fields</term>
    ///                 <description>Marked with NixInjectAttribute will be populated with NixiContainer</description>
    ///             </item>
    ///         </list>
    ///     </item>
    ///     
    ///     <item>
    ///         <term>InjectionTestTemplate usage</term>
    ///         <description>TestInjecter passed in BuildInjections</description>
    ///     </item>
    ///     <item>
    ///         <list type="bullet">
    ///             <item>
    ///                 <term>Component fields</term>
    ///                 <description>Marked with NixInjectComponentAttribute will be created, used to populate the field and registered in TestInjecter property of InjectionTestTemplate</description>
    ///             </item>
    ///             <item>
    ///                 <term>Non-Component fields</term>
    ///                 <description>Marked with NixInjectAttribute will be mockable, you can inject mock in it</description>
    ///             </item>
    ///         </list>
    ///     </item>
    /// </list>
    /// </summary>
    public abstract class MonoBehaviourInjectable : MonoBehaviour
    {
        protected virtual NixInjectOptions NixInjectOptions => null;

        [ExcludeFromCodeCoverage] // Cannot be tested, protected Awake
        protected virtual void Awake()
        {
            new NixInjecter(this, NixInjectOptions).CheckAndInjectAll();
        }
    }
}