using Nixi.Injections;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Builders
{
    /// <summary>
    /// Builder for FieldInfo of Enumerable components, this one init all FieldInfos with a number of component added on every 
    /// FieldInfos that match the same EnumerableType and level. If it has already been initialized, it will throw an exception.
    /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
    /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
    /// </summary>
    internal sealed class EnumerableComponentBuilderNumberAdded : EnumerableComponentBuilder<int>
    {
        /// <summary>
        /// Build a number of components of type T (derived from Component) at GameObjectLevel
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Targeted type</typeparam>
        /// <param name="gameObjectLevel">Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached </param>
        /// <param name="injectable">MonoBehaviourInjectable to inject</param>
        /// <param name="nbAdded">Number of components added</param>
        /// <returns>All components built</returns>
        protected override List<T> BuildComponents<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, int nbAdded)
        {
            List<T> components = new List<T>();
            for (int i = 0; i < nbAdded; i++)
            {
                Component componentBuilded = BuildComponentForEnumerableComponentFields(gameObjectLevel, injectable, typeof(T));
                components.Add((T)componentBuilded);
            }
            return components;
        }
    }
}