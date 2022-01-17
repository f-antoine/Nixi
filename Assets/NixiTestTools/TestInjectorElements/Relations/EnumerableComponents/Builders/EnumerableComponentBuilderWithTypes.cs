using Nixi.Injections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Builders
{
    /// <summary>
    /// Builder for FieldInfo of Enumerable components, this one init all FieldInfos with an array of type derived from targeted derived 
    /// type from component. For each type passed in the array number of component, one of this type is generated and added into every FieldInfo 
    /// concerned. If it has already been initialized, it will throw an exception.
    /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
    /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
    /// </summary>
    internal sealed class EnumerableComponentBuilderWithTypes : EnumerableComponentBuilder<Type[]>
    {
        /// <summary>
        /// Build as several components as derived types passed as parameters at GameObjectLevel
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Targeted type</typeparam>
        /// <param name="gameObjectLevel">Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached </param>
        /// <param name="injectable">MonoBehaviourInjectable to inject</param>
        /// <param name="typesDerived">All types derived from T to build</param>
        /// <returns>All components built</returns>
        protected override List<T> BuildComponents<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type[] typesDerived)
        {
            List<T> components = new List<T>();
            Type typeToFind = typeof(T);

            foreach (Type typeDerived in typesDerived)
            {
                if (!typeToFind.IsAssignableFrom(typeDerived))
                    throw new InjectablesContainerException($"{typeToFind.Name} is not assignable from {typeDerived.Name}");

                Component componentBuilded = BuildComponentForEnumerableComponentFields(gameObjectLevel, injectable, typeDerived);
                components.Add((T)componentBuilded);
            }

            return components;
        }
    }
}