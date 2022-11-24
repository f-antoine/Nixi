using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.Enums;
using Nixi.Injections.Injectors;
using NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Builders
{
    /// <summary>
    /// Base class for builders of FieldInfo containing Enumerable components, it inits all fieldInfos that match the same EnumerableType and 
    /// level. If it has already been initialized, it will throw an exception.
    /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
    /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
    /// </summary>
    internal abstract class EnumerableComponentBuilder<TParam>
    {
        /// <summary>
        /// Build several components of type T (derived from Component) at GameObjectLevel
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Targeted type</typeparam>
        /// <param name="gameObjectLevel">Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached </param>
        /// <param name="injectable">MonoBehaviourInjectable to inject</param>
        /// <param name="param">Param to defined how to populate with several components</param>
        /// <returns>All components built</returns>
        protected abstract List<T> BuildComponents<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, TParam param)
            where T : Component;

        /// <summary>
        /// Find all the ComponentListWithFieldInfos that match type T and GameObjectLevel, then check if they are all empty.
        /// Then, build several components of type T (derived from Component) and fill ComponentListWithFieldInfos with all these generated components
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Type of component (enumerable type)</typeparam>
        /// <param name="fields">All field contained in the current EnumerableComponentRelationHandler</param>
        /// <param name="gameObjectLevel">Targeted scope for enumerable field</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <param name="param">Param to defined how to populate with several components</param>
        /// <returns>Components of type T generated</returns>
        internal List<T> BuildAndEnumerableComponentFields<T>(IReadOnlyList<ComponentListWithFieldInfo> fields, GameObjectLevel gameObjectLevel,
                                                                MonoBehaviourInjectable injectable, TParam param)
            where T : Component
        {
            IEnumerable<ComponentListWithFieldInfo> enumerableFieldsData = GetAllEnumerableComponentsWithCriteria<T>(fields, gameObjectLevel);
            CheckAllEnumerableComponentNotAlreadyInitiated<T>(enumerableFieldsData, injectable);

            List<T> components = BuildComponents<T>(gameObjectLevel, injectable, param);

            foreach (ComponentListWithFieldInfo enumerableFieldData in enumerableFieldsData)
            {
                enumerableFieldData.FieldInfo.SetValue(injectable, components, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
            }

            return components;
        }

        /// <summary>
        /// Get all ComponentListWithFieldInfo instantiated in a MonoBehaviourInjectable field which match an enumerable of type T 
        /// (inherited from Component) and a GameObjectLevel
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component</typeparam>
        /// <param name="fields">All field contained in the current EnumerableComponentRelationHandler</param>
        /// <param name="gameObjectLevel">Targeted level (children, parents or current component level list)</param>
        /// <returns>List of component instantiated which corresponds to type T field</returns>
        protected IEnumerable<ComponentListWithFieldInfo> GetAllEnumerableComponentsWithCriteria<T>(IReadOnlyList<ComponentListWithFieldInfo> fields, GameObjectLevel gameObjectLevel)
            where T : Component
        {
            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));

            if (!componentsListWithType.Any())
                throw new EnumerableComponentBuilderException($"with type {typeToFind.Name} was found");

            IEnumerable<ComponentListWithFieldInfo> componentWithCriteria = componentsListWithType.Where(x => x.GameObjectLevel == gameObjectLevel);

            if (!componentWithCriteria.Any())
                throw new EnumerableComponentBuilderException($"with type {typeToFind.Name} and GameObjectLevel equals to {gameObjectLevel} was found");

            return componentWithCriteria;
        }

        /// <summary>
        /// Check if an Enumerable of component field (or a set of component field enumerable) has already been initiliazed
        /// </summary>
        /// <typeparam name="T">IEnumerable type to find for the cast into IEnumerable</typeparam>
        /// <param name="componentsWithCriteria">All ComponentListWithFieldInfo tested</param>
        /// <param name="injectable">Injectable on which we are looking for the fieldInfo value contained</param>
        /// <exception cref="InjectablesContainerException">Thrown in any componentsWithCriteria.FieldInfo.GetValue is not empty</exception>
        protected void CheckAllEnumerableComponentNotAlreadyInitiated<T>(IEnumerable<ComponentListWithFieldInfo> componentsWithCriteria, MonoBehaviourInjectable injectable)
            where T : Component
        {
            Type typeToFind = typeof(T);
            foreach (var element in componentsWithCriteria)
            {
                if (element.EnumerableType == typeToFind)
                {
                    object value = element.FieldInfo.GetValue(injectable);

                    IEnumerable<T> enumerableValue = EnumerableTools.GetEnumerableFromObject<T>(value);

                    if (enumerableValue.Any())
                        throw new InjectablesContainerException($"cannot init list of type {typeToFind.Name} twice");
                }
            }
        }

        /// <summary>
        /// Create a component of type targetedType
        /// <para/>If this is at same level of the injectable instance, this is directly added to its gameObject
        /// <para/>If not, a new gameObject is built
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">Injectable for which we are adding a component</param>
        /// <param name="targetedType">Type of component wanted (inherited from Component)</param>
        /// <returns>New component</returns>
        protected Component BuildComponentForEnumerableComponentFields(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type targetedType)
        {
            Component newComponent;

            if (gameObjectLevel == GameObjectLevel.Current)
            {
                // This is the same gameObject of monoBehaviourInjectableToFind and we only AddComponent of type wanted
                newComponent = injectable.gameObject.AddComponent(targetedType);
            }
            else
            {
                // Other cases, we create a new GameObject and AddComponent of type wanted
                newComponent = new GameObject().AddComponent(targetedType);
            }

            return newComponent;
        }
    }
}
