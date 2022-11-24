using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.Enums;
using NixiTestTools.TestInjectorElements.Relations.Abstractions;
using NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Builders;
using NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents
{
    /// <summary>
    /// Contains all relations between List/IEnumerable/array of components (parent/children links) and keep an access to all their FieldInfo (wrapped into ComponentListWithFieldInfo)
    /// to handle all injections of theses relations
    /// </summary>
    internal sealed class EnumerableComponentRelationHandler : RelationHandlerBase<IEnumerable<Component>, ComponentListWithFieldInfo>
    {
        /// <summary>
        /// Name to use in exception thrown to be more precise about the type of field targeted
        /// </summary>
        protected override string FieldTypeName => "enumerable component";

        /// <summary>
        /// Method used to find a FieldHandled into the list of FieldHandled
        /// <para/>Here this is based on EnumerableType (genericType/elementType if array) instead of FieldInfo.FieldType directly.
        /// This helps simulate Unity dependency injection, if you populates an IEnumerable, a List or an array at same level (current/child/parent) 
        /// you will have the same values in all of them.
        /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        protected override Func<ComponentListWithFieldInfo, Type, bool> MethodToFindFieldHandled
            => (x, typeToFind) => x.EnumerableType.IsAssignableFrom(typeToFind);

        /// <summary>
        /// Builder for FieldInfo of Enumerable components, this one init all FieldInfos with a number of component added on every 
        /// FieldInfos that match the same EnumerableType and level. If it has already been initialized, it will throw an exception.
        /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        private EnumerableComponentBuilderNumberAdded enumerableComponentBuildNumberAdded = new EnumerableComponentBuilderNumberAdded();

        /// <summary>
        /// Builder for FieldInfo of Enumerable components, this one init all FieldInfos with an array of type derived from targeted derived 
        /// type from component. For each type passed in the array number of component, one of this type is generated and added into every FieldInfo 
        /// concerned. If it has already been initialized, it will throw an exception.
        /// <para/>It means, they are generated and linked only once and the values are reused everywhere at the same level where the EnumerableType is the same
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        private EnumerableComponentBuilderWithTypes enumerableComponentBuilderWithTypes = new EnumerableComponentBuilderWithTypes();

        /// <summary>
        /// Builds nbAdded component(s) at targeted GameObjectLevel
        /// <para/>This is based on T type == EnumerableType (genericType/elementType if array)
        /// This helps simulate Unity dependency injection, if you populates an IEnumerable, a List or an array at same level (current/child/parent) 
        /// you will have the same values in all of them.
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Type of component (enumerable type)</typeparam>
        /// <param name="gameObjectLevel">Targeted scope for enumerable field</param>
        /// <param name="nbAdded">Number to build</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Components of type T generated</returns>
        internal List<T> BuildManyEnumerableComponents<T>(GameObjectLevel gameObjectLevel, int nbAdded, MonoBehaviourInjectable injectable)
            where T : Component
        {
            try
            {
                return enumerableComponentBuildNumberAdded.BuildAndEnumerableComponentFields<T>(Fields, gameObjectLevel, injectable, nbAdded);
            }
            catch (EnumerableComponentBuilderException exception)
            {
                throw new InjectablesContainerException($"no {FieldTypeName} {exception.Message}");
            }
        }

        /// <summary>
        /// Builds one component for each typesDerived at targeted GameObjectLevel, those typesDerived must be inherited from type of T 
        /// (which themselves are derived from component)
        /// <para/>This is based on T type == EnumerableType (genericType/elementType if array)
        /// This helps simulate Unity dependency injection, if you populates an IEnumerable, a List or an array at same level (current/child/parent) 
        /// you will have the same values in all of them.
        /// <para/><see cref="GameObjectLevel">Check here to have more information about "levels"</see>
        /// </summary>
        /// <typeparam name="T">Type of component (enumerable type)</typeparam>
        /// <param name="gameObjectLevel">Targeted scope for enumerable field</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <param name="typesDerived">All types derived from T to build</param>
        /// <returns>Components of type T generated</returns>
        internal List<T> BuildEnumerableComponentsWithTypes<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type[] typesDerived)
            where T : Component
        {
            try
            {
                return enumerableComponentBuilderWithTypes.BuildAndEnumerableComponentFields<T>(Fields, gameObjectLevel, injectable, typesDerived);
            }
            catch (EnumerableComponentBuilderException exception)
            {
                throw new InjectablesContainerException($"no {FieldTypeName} {exception.Message}");
            }
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type T
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(MonoBehaviourInjectable injectable)
            where T : Component
        {
            ComponentListWithFieldInfo fieldHandler = GetFieldInfoHandler(typeof(T));

            object result = fieldHandler.FieldInfo.GetValue(injectable);

            return EnumerableTools.GetEnumerableFromObject<T>(result);
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type T and fieldName
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="fieldName">Name of the fields targeted</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(string fieldName, MonoBehaviourInjectable injectable)
            where T : Component
        {
            ComponentListWithFieldInfo fieldHandler = GetFieldInfoHandler(typeof(T), fieldName);

            object result = fieldHandler.FieldInfo.GetValue(injectable);

            return EnumerableTools.GetEnumerableFromObject<T>(result);
        }
    }
}