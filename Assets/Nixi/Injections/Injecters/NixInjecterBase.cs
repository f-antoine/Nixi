﻿using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Injecters
{
    /// <summary>
    /// Base injector used to fill fields decorated with Nixi attributes of a MonoBehaviourInjectable
    /// </summary>
    public abstract class NixInjecterBase
    {
        /// <summary>
        /// Instance of the class derived from MonoBehaviourInjectable on which all fields with Nixi inject attributes will be injected
        /// </summary>
        protected readonly MonoBehaviourInjectable objectToLink;

        /// <summary>
        /// Flag that defines if the injection has already been done, if so, it can no longer be injected
        /// </summary>
        public bool IsInjected { get; private set; } = false;

        /// <summary>
        /// Predicate to find all NixInjectAttribute on a Non-Component field in a MonoBehaviourInjectable
        /// </summary>
        protected Func<FieldInfo, bool> NixiFieldPredicate => x => x.CustomAttributes.Any(nonComponentFieldsPredicate);

        /// <summary>
        /// Predicate to find all attributes derived from NixInjectComponentBaseAttribute on a Component field in a MonoBehaviourInjectable
        /// </summary>
        protected Func<FieldInfo, bool> NixiComponentFieldPredicate => x => x.CustomAttributes.Any(ComponentFieldsPredicate);

        /// <summary>
        /// Predicate to identify a NixInjectAttribute on a CustomAttributeData
        /// </summary>
        private Func<CustomAttributeData, bool> nonComponentFieldsPredicate => y => y.AttributeType == typeof(NixInjectAttribute);

        /// <summary>
        /// Predicate to identify all attributes derived from NixInjectComponentBaseAttribut on a CustomAttributeData
        /// </summary>
        private Func<CustomAttributeData, bool> ComponentFieldsPredicate => y => typeof(NixInjectComponentBaseAttribute).IsAssignableFrom(y.AttributeType);

        /// <summary>
        /// Combination of nonComponentFieldsPredicate and componentFieldsPredicate
        /// </summary>
        protected Func<CustomAttributeData, bool> AllNixiFieldsPredicate => y => y.AttributeType == typeof(NixInjectAttribute)
                                                                            || typeof(NixInjectComponentBaseAttribute).IsAssignableFrom(y.AttributeType);

        /// <summary>
        /// Base injector used to fill fields decorated with Nixi attributes of a MonoBehaviourInjectable
        /// </summary>
        /// <param name="objectToLink">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        public NixInjecterBase(MonoBehaviourInjectable objectToLink)
        {
            this.objectToLink = objectToLink;
        }

        /// <summary>
        /// Check if InjectAll has never been get called and inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in objectToLink
        /// </summary>
        /// <exception cref="NixInjecterException">Thrown if this method has already been called</exception>
        public virtual void CheckAndInjectAll()
        {
            if (IsInjected)
                throw new NixInjecterException($"InjectAll has already been called and cannot be called more than once", objectToLink);

            InjectAll();

            IsInjected = true;
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in objectToLink
        /// </summary>
        protected abstract void InjectAll();

        /// <summary>
        /// Find all the fields (public, protected and private) contained in the objectToLink type, this goes back recursively to the first type derived from MonoBehaviourInjectable
        /// </summary>
        /// <returns>All the fields contained in objectToLink type</returns>
        protected virtual List<FieldInfo> GetAllFields(Type currentType)
        {
            List<FieldInfo> fieldsToReturn = new List<FieldInfo>();

            string lastType = typeof(MonoBehaviourInjectable).Name;
            while (currentType.Name != lastType)
            {
                // BindingFlags.DeclaredOnly to get only fields at the level scanned and avoid inheritance duplication on public and protected fields
                foreach (FieldInfo element in currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    CheckDecoratedWithOnlyOneNixiAttribute(element);
                    fieldsToReturn.Add(element);
                }

                currentType = currentType.BaseType;
            }

            return fieldsToReturn;
        }

        /// <summary>
        /// Check if a FieldInfo type is not a Component
        /// </summary>
        /// <param name="nonComponentField">Field to check</param>
        protected void CheckIsNotComponent(FieldInfo nonComponentField)
        {
            if (typeof(Component).IsAssignableFrom(nonComponentField.FieldType))
                throw new NixInjecterException($"Cannot register field with name {nonComponentField.Name} with a NixInjectAttribute because it is a Component field, you must use NixInjectComponentAttribute instead", objectToLink);
        }

        /// <summary>
        /// Check if a FieldInfo type is a Component
        /// </summary>
        /// <param name="componentField">Field to check</param>
        protected void CheckIsComponent(FieldInfo componentField)
        {
            if (!typeof(Component).IsAssignableFrom(componentField.FieldType))
                throw new NixInjecterException($"Cannot inject field with name {componentField.Name} with a NixInjectComponentAttribute because it is not a Component field, you must use NixInjectAttribute instead", objectToLink);
        }

        /// <summary>
        /// Check if a fieldInfo is decorated by only one Nixi attribute (NixInjectAttribute or derived from NixInjectComponentBaseAttribute)
        /// <para/>If there is more than one attribute, it throws an exception
        /// </summary>
        /// <param name="fieldInfo">Field info to check</param>
        protected void CheckDecoratedWithOnlyOneNixiAttribute(FieldInfo fieldInfo)
        {
            IEnumerable<CustomAttributeData> nbNixAttributes = fieldInfo.CustomAttributes.Where(AllNixiFieldsPredicate);

            if (nbNixAttributes.Count() > 1)
            {
                string attributeElementsDetailed = string.Join(", ", fieldInfo.CustomAttributes.Select(x => x.AttributeType.Name));
                throw new NixInjecterException($"Cannot inject {fieldInfo.Name} because there is more than one Nixi attribute decorating it, list of attributes decorating this field : {attributeElementsDetailed}", objectToLink);
            }
        }
    }
}