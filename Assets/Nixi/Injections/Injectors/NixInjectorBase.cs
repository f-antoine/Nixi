using Nixi.Injections.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Injectors
{
    /// <summary>
    /// Base Injector used to fill fields decorated with Nixi attributes of a MonoBehaviourInjectable
    /// </summary>
    public abstract class NixInjectorBase
    {
        /// <summary>
        /// Options available to parameterized the injections
        /// </summary>
        private NixInjectOptions nixInjectOptions;

        /// <summary>
        /// Instance of the class derived from MonoBehaviourInjectable on which all fields with Nixi attributes will be injected
        /// </summary>
        protected readonly MonoBehaviourInjectable mainInjectable;

        /// <summary>
        /// Flag that defines if the injection has already been done, if so, it can no longer be injected
        /// <exception cref="NixInjectorException">Thrown if CheckAndInjectAll has already been called</exception>
        /// </summary>
        public bool IsInjected { get; private set; } = false;

        #region Predicates
        /// <summary>
        /// Predicate to find all attributes derived from NixInjectBaseAttribute on a Non-Component field in a MonoBehaviourInjectable
        /// </summary>
        protected Func<FieldInfo, bool> NixiFieldPredicate => 
            x => x.CustomAttributes.Any(y =>
            {
                return typeof(NixInjectBaseAttribute).IsAssignableFrom(y.AttributeType) || typeof(SerializeField).IsAssignableFrom(y.AttributeType);
            }
        );

        /// <summary>
        /// Predicate to find all attributes derived from NixInjectComponentBaseAttribute on a Component field in a MonoBehaviourInjectable
        /// </summary>
        protected Func<FieldInfo, bool> NixiComponentFieldPredicate =>
            x => x.CustomAttributes.Any(y =>
            {
                return typeof(NixInjectComponentBaseAttribute).IsAssignableFrom(y.AttributeType);
            }
        );

        /// <summary>
        /// Predicate to find all NixiAttribute (component and non component fields)
        /// </summary>
        protected Func<CustomAttributeData, bool> AllNixiFieldsPredicate => y => typeof(NixInjectAbstractBaseAttribute).IsAssignableFrom(y.AttributeType);

        /// <summary>
        /// Predicate to identify fields decorated with UnityEngine.SerializeField attribute
        /// </summary>
        private Func<CustomAttributeData, bool> serializeFieldsPredicate => y => typeof(SerializeField).IsAssignableFrom(y.AttributeType);
        #endregion Predicates

        /// <summary>
        /// Base injector used to populate fields decorated with Nixi attributes of a MonoBehaviourInjectable
        /// </summary>
        /// <param name="mainInjectable">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        /// <param name="nixInjectOptions">Options available to parameterized the injections, if null this is the default behavior</param>
        public NixInjectorBase(MonoBehaviourInjectable mainInjectable, NixInjectOptions nixInjectOptions = null)
        {
            this.mainInjectable = mainInjectable;
            this.nixInjectOptions = nixInjectOptions ?? new NixInjectOptions();
        }

        /// <summary>
        /// Check if CheckAndInjectAll has never been get called and inject all the fields decorated with Nixi Attributes in mainInjectable
        /// </summary>
        /// <exception cref="NixInjectorException">Thrown if this method has already been called</exception>
        public virtual void CheckAndInjectAll()
        {
            if (IsInjected)
                throw new NixInjectorException($"CheckAndInjectAll has already been called and cannot be called more than once", mainInjectable);

            InjectAll();

            IsInjected = true;
        }

        /// <summary>
        /// Inject all the fields decorated by Nixi attributes in mainInjectable
        /// </summary>
        protected abstract void InjectAll();

        /// <summary>
        /// Find all the fields (public, protected and private) contained in the mainInjectable type, this goes back recursively to the first type derived from MonoBehaviourInjectable
        /// </summary>
        /// <returns>All the fields contained in mainInjectable type</returns>
        protected virtual List<FieldInfo> GetAllFields(Type currentType)
        {
            List<FieldInfo> fieldsToReturn = new List<FieldInfo>();

            string lastType = typeof(MonoBehaviourInjectable).Name;
            while (currentType.Name != lastType)
            {
                // BindingFlags.DeclaredOnly to get only fields at the level scanned and avoid inheritance duplication on public and protected fields
                foreach (FieldInfo element in currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    CheckDecoratedWithOnlyOneNixiAttributeOrSerializeField(element);
                    fieldsToReturn.Add(element);
                }

                currentType = currentType.BaseType;
            }

            return fieldsToReturn;
        }

        /// <summary>
        /// Check if a fieldInfo is decorated by only one Nixi attribute (NixInjectAttribute or derived from NixInjectComponentBaseAttribute)
        /// </summary>
        /// <param name="fieldInfo">Field info to check</param>
        /// <exception cref="NixInjectorException">Thrown if there is more than one attribute, it throws an exception</exception>
        protected void CheckDecoratedWithOnlyOneNixiAttributeOrSerializeField(FieldInfo fieldInfo)
        {
            int nbTargetedAttributes = fieldInfo.CustomAttributes.Count(AllNixiFieldsPredicate);

            if (!nixInjectOptions.AuthorizeSerializedFieldWithNixiAttributes)
            {
                nbTargetedAttributes += fieldInfo.CustomAttributes.Count(serializeFieldsPredicate);
            }

            if (nbTargetedAttributes > 1)
            {
                string attributeElementsDetailed = string.Join(", ", fieldInfo.CustomAttributes.Select(x => x.AttributeType.Name));
                throw new NixInjectorException($"Cannot inject {fieldInfo.Name} because there is more than one Nixi attribute decorating it (or a Nixi attribute was combined " +
                                               $"with SerializeField), list of attributes decorating this field : {attributeElementsDetailed}", mainInjectable);
            }
        }
    }
}