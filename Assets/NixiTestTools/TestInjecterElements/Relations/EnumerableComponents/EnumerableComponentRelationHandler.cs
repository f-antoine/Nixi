using Nixi.Injections;
using Nixi.Injections.Attributes;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.EnumerableComponents
{
    /// <summary>
    /// Contains all relations between components of type "list of component" (parent/child chain) and handle all FieldInfo injections of theses relations
    /// </summary>
    internal sealed class EnumerableComponentRelationHandler : RelationHandlerBase<IEnumerable<Component>, ComponentListWithFieldInfo>
    {
        /// <summary>
        /// Name to use in exception thrown to be more precise about the type of field targeted
        /// </summary>
        protected override string fieldTypeName => "enumerable component";

        /// <summary>
        /// Add a field to the list of fields of type ComponentWithFieldInfo and add a relation with his component at top parent
        /// </summary>
        /// <param name="fieldToAdd">Field to add</param>
        internal override void AddFieldAndLink(ComponentListWithFieldInfo fieldToAdd)
        {
            AddRelation(fieldToAdd.Components);
            base.AddFieldAndLink(fieldToAdd);
        }

        /// <summary>
        /// Method to use in Where condition on fields to match typeToFind
        /// </summary>
        /// <param name="typeToFind">FieldType of FieldInfo to find</param>
        /// <returns>Single FieldHandled with type equals to typeToFind</returns>
        protected override Func<ComponentListWithFieldInfo, bool> FieldMatchType(Type typeToFind)
        {
            return x => x.EnumerableType.IsAssignableFrom(typeToFind);
        }

        /// <summary>
        /// Get all ComponentListWithFieldInfo instantiated in a MonoBehaviourInjectable field which match an enumerable of type T (inherited from Component) and a GameObjectLevel
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <returns>List of component instantiated which corresponds to type T field</returns>
        internal IEnumerable<ComponentListWithFieldInfo> GetAllEnumerableComponentsWithCriteria<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable)
            where T : Component
        {
            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = Fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));

            if (!componentsListWithType.Any())
                throw new InjectablesContainerException($"no {fieldTypeName} with type {typeToFind.Name} was found");

            IEnumerable<ComponentListWithFieldInfo> componentWithCriteria = componentsListWithType.Where(x => x.EnumerableType == typeToFind && x.GameObjectLevel == gameObjectLevel);
            
            if (!componentWithCriteria.Any())
                throw new InjectablesContainerException($"no {fieldTypeName} with type {typeToFind.Name} and GameObjectLevel {gameObjectLevel} was found");

            CheckEnumerableComponentNotAlreadyInitiated<T>(componentWithCriteria, injectable);

            return componentWithCriteria;
        }

        /// <summary>
        ///  Check if a component field enumerable (or a set of component field enumerable) has already been initiliazed (FieldInfo.GetValue is not empty it throws an exception)
        /// </summary>
        /// <typeparam name="T">IEnumerable type to find for the cast into IEnumerable</typeparam>
        /// <param name="componentsWithCriteria">All ComponentListWithFieldInfo tested</param>
        /// <param name="injectable">Injectable on which we are looking for the fieldInfo value contained</param>
        private void CheckEnumerableComponentNotAlreadyInitiated<T>(IEnumerable<ComponentListWithFieldInfo> componentsWithCriteria, MonoBehaviourInjectable injectable)
            where T : Component
        {
            Type typeToFind = typeof(T);
            foreach (var element in componentsWithCriteria)
            {
                if (element.EnumerableType == typeToFind)
                {
                    object value = element.FieldInfo.GetValue(injectable);

                    IEnumerable<T> enumerableValue = GetEnumerableFromObject<T>(value);

                    if (enumerableValue.Any())
                        throw new InjectablesContainerException($"Cannot init list of type {typeToFind.Name} twice");
                }
            }
        }

        /// <summary>
        /// Builds the nbAdded component
        /// </summary>
        /// <typeparam name="T">Type of component (enumerable type)</typeparam>
        /// <param name="gameObjectLevel">Targeted scope for enumerable field</param>
        /// <param name="nbAdded">Number to build</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Components of type T generated</returns>
        internal List<T> BuildManyEnumerableComponents<T>(GameObjectLevel gameObjectLevel, int nbAdded, MonoBehaviourInjectable injectable)
            where T : Component
        {
            List<T> components = new List<T>();
            for (int i = 0; i < nbAdded; i++)
            {
                Component componentBuilded = BuildComponentForEnumerableComponentFields(gameObjectLevel, injectable, typeof(T));
                components.Add((T)componentBuilded);
            }

            return components;
        }

        /// <summary>
        /// Builds one component for each typeDeriveds, those typeDeriveds must be inherited from type of T
        /// </summary>
        /// <typeparam name="T">Type of component (enumerable type)</typeparam>
        /// <param name="gameObjectLevel">Targeted scope for enumerable field</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <param name="typeDeriveds">All type derived from T we want to build</param>
        /// <returns>Components of type T generated</returns>
        internal List<T> BuildEnumerableComponentsWithTypes<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type[] typeDeriveds)
            where T : Component
        {
            List<T> components = new List<T>();
            Type typeToFind = typeof(T);

            foreach (Type typeDerived in typeDeriveds)
            {
                if (!typeToFind.IsAssignableFrom(typeDerived))
                    throw new InjectablesContainerException($"{typeToFind.Name} is not assignable from {typeDerived.Name}");

                Component componentBuilded = BuildComponentForEnumerableComponentFields(gameObjectLevel, injectable, typeDerived);
                components.Add((T)componentBuilded);
            }

            return components;
        }

        /// <summary>
        /// Create a component of type targetType, if this is at same level of the injectable instance, this is directly added to his gameObject, if not, a new gameObject is built
        /// </summary>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">Injectable for which we are adding a component</param>
        /// <param name="targetedType">Type of component wanted (inherited from Component)</param>
        /// <returns>New component</returns>
        private Component BuildComponentForEnumerableComponentFields(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type targetedType)
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

        /// <summary>
        /// Transpose an object into a IEnumerable with generic type T derived from Component
        /// </summary>
        /// <typeparam name="T">Enumerable generic type</typeparam>
        /// <param name="value">Object to convert</param>
        /// <returns>IEnumerable with generic type T</returns>
        internal IEnumerable<T> GetEnumerableFromObject<T>(object value)
            where T : Component
        {
            if (value.GetType().IsArray)
            {
                var valueEnumerable = (System.Collections.IEnumerable)value;

                List<T> objectsToConvert = new List<T>();
                foreach (var element in valueEnumerable)
                {
                    objectsToConvert.Add((T)element);
                }

                return objectsToConvert.ToArray();
            }
            return (IEnumerable<T>)value;
        }
    }
}