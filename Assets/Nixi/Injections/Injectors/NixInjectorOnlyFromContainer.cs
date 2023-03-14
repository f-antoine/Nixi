using Nixi.Containers;
using Nixi.Injections.Attributes;
using Nixi.Injections.Attributes.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nixi.Injections.Injectors
{
    // TODO : Recheck if useful
    // TODO : Comment
    public class NixInjectorOnlyFromContainer : NixInjectorBase<OnlyFromContainerInjectable>
    {
        // TODO : Comment
        protected Func<FieldInfo, bool> NixInjectFromContainerPredicate =>
            x => x.CustomAttributes.Any(y =>
            {
                return typeof(FromContainerAttribute).IsAssignableFrom(y.AttributeType);
            }
        );

        // TODO : Comment
        public NixInjectorOnlyFromContainer(OnlyFromContainerInjectable mainInjectable)
            : base(mainInjectable)
        {
        }

        // TODO : Comment
        protected override void InjectAll()
        {
            IEnumerable<FieldInfo> fields = GetAllFields(mainInjectable.GetType());

            try
            {
                InjectContainerFields(fields.Where(NixInjectFromContainerPredicate));
            }
            catch (NixiAttributeException exception)
            {
                throw new NixInjectorException(exception.Message, mainInjectable.ToString(), mainInjectable.GetType());
            }
        }

        // TODO : Comment
        private void InjectContainerFields(IEnumerable<FieldInfo> containerFields)
        {
            foreach (FieldInfo containerField in containerFields)
            {
                InjectContainerField(containerField);
            }
        }

        // TODO : Check to merge InjectFields (TestInjector, and other ?)
        // TODO : Comment
        private void InjectContainerField(FieldInfo containerField)
        {
            FromContainerAttribute injectAttribute = containerField.GetCustomAttribute<FromContainerAttribute>();
            injectAttribute.CheckIsValidAndBuildDataFromField(containerField);

            object resolved = NixiContainer.ResolveMap(containerField.FieldType);
            containerField.SetValue(mainInjectable, resolved);
        }

        protected override void CheckFieldDecorators(FieldInfo fieldInfo)
        {
            int nbTargetedAttributes = fieldInfo.CustomAttributes.Count(AllNixiFieldsPredicate);
            nbTargetedAttributes += fieldInfo.CustomAttributes.Count(SerializeFieldsPredicate);

            if (nbTargetedAttributes > 1)
            {
                string attributeElementsDetailed = string.Join(", ", fieldInfo.CustomAttributes.Select(x => x.AttributeType.Name));
                throw new NixInjectorException($"Cannot inject {fieldInfo.Name} because there is more than one Nixi attribute decorating it " +
                                               $"(or a Nixi attribute was combined with SerializeField), list of attributes decorating this field : {attributeElementsDetailed}",
                                               mainInjectable.ToString(), mainInjectable.GetType());
            }
            else if (nbTargetedAttributes == 1
                     && fieldInfo.GetCustomAttribute<FromContainerAttribute>() == null)
            {
                throw new NixInjectorException($"Cannot inject {fieldInfo.Name} because the single NixiAttribute is not NixInjectFromContainerAttribute " +
                                               $"which is the only one allowed in NixInjectorOnlyFromContainer",
                                               mainInjectable.ToString(), mainInjectable.GetType());
            }
        }
    }
}