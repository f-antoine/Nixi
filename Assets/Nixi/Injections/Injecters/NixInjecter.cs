using Nixi.Containers;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Injecters
{
    /// <summary>
    /// Default way to handle all injections of fields decorated with Nixi inject attributes of a class derived from MonoBehaviourInjectable during play mode
    /// <list type="bullet">
    ///     <item>
    ///         <term>MonoBehaviour fields</term>
    ///         <description>Marked with NixInjectMonoBehaviourAttribute will be populated with Unity dependency injection</description>
    ///     </item>
    ///     <item>
    ///         <term>Non-MonoBehaviour fields</term>
    ///         <description>Marked with NixInjectAttribute will be populated with NixiContainer</description>
    ///     </item>
    /// </list>
    /// </summary>
    public sealed class NixInjecter : NixInjecterBase
    {
        /// <summary>
        /// Default way to handle all injections of fields decorated with Nixi attributes of a class derived from MonoBehaviourInjectable during play mode
        /// </summary>
        /// <param name="objectToLink">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        public NixInjecter(MonoBehaviourInjectable objectToLink)
            : base(objectToLink)
        {
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectMonoBehaviourAttribute in objectToLink
        /// </summary>
        /// <exception cref="NixInjecterException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            List<FieldInfo> fields = GetAllFields(objectToLink.GetType());

            InjectFields(fields.Where(NixiFieldPredicate));
            InjectMonoBehaviourFields(fields.Where(NixiMonoBehaviourFieldPredicate));
        }

        #region Non-Monobehaviour Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectAttribute (non-MonoBehaviour fields) and with NixInjectType set to FillWithContainer 
        /// </summary>
        /// <param name="nonMonoBehaviourFields">Non-MonoBehaviour fields</param>
        private void InjectFields(IEnumerable<FieldInfo> nonMonoBehaviourFields)
        {
            foreach (FieldInfo field in nonMonoBehaviourFields)
            {
                NixInjectAttribute injectAttribute = field.GetCustomAttribute<NixInjectAttribute>();

                if (injectAttribute.NixInjectType == NixInjectType.FillWithContainer)
                    InjectField(field);
            }
        }

        /// <summary>
        /// Fill a non-MonoBehaviour field in the objectToLink with the mapping resolve from the NixiContainer
        /// </summary>
        /// <param name="nonMonoBehaviourField">Non-MonoBehaviour field</param>
        private void InjectField(FieldInfo nonMonoBehaviourField)
        {
            CheckIsNotMonoBehaviour(nonMonoBehaviourField);

            if (!nonMonoBehaviourField.FieldType.IsInterface)
                throw new NixInjecterException($"The field with the name {nonMonoBehaviourField.Name} with a NixInjectAttribute must be an interface " +
                    $"because the container works with only interfaces as a key for injection, " +
                    $"if you don't want to use the container and only expose for the tests from template, " +
                    $"you can use DoesNotFillButExposeForTesting option on the attribute constructor", objectToLink);

            object resolved = NixiContainer.Resolve(nonMonoBehaviourField.FieldType);
            nonMonoBehaviourField.SetValue(objectToLink, resolved);
        }
        #endregion Non-Monobehaviour Injections

        #region Monobehaviour Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectMonoBehaviourAttribute (MonoBehaviour fields)
        /// </summary>
        /// <param name="monoBehaviourFields">MonoBehaviour fields</param>
        private void InjectMonoBehaviourFields(IEnumerable<FieldInfo> monoBehaviourFields)
        {
            foreach (FieldInfo monoBehaviourField in monoBehaviourFields)
            {
                InjectGameObject(monoBehaviourField);
            }
        }

        /// <summary>
        /// Fill a MonoBehaviour field in the objectToLink with the Unity dependency injection method which corresponds to the value of NixInjectMonoBehaviourAttribute.GameObjectMethod in GameObjectMethodBindings
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        private void InjectGameObject(FieldInfo monoBehaviourField)
        {
            CheckIsMonoBehaviour(monoBehaviourField);

            NixInjectMonoBehaviourBaseAttribute injectAttribute = monoBehaviourField.GetCustomAttribute<NixInjectMonoBehaviourBaseAttribute>();
            Component componentToTranspose = injectAttribute.GetSingleComponent(objectToLink, monoBehaviourField.FieldType);
            monoBehaviourField.SetValue(objectToLink, componentToTranspose);
        }
        #endregion Monobehaviour Injections
    }
}