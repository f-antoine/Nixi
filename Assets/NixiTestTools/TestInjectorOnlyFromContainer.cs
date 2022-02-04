using Nixi.Injections;
using Nixi.Injections.Injectors;
using NixiTestTools.TestInjectorElements;
using NixiTestTools.TestInjectorElements.Relations.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NixiTestTools
{
    // TODO : Check if this must be derived from NixInjectorOnlyFromContainer
    // TODO : Comment
    public sealed class TestInjectorOnlyFromContainer : NixInjectorOnlyFromContainer
    {
        // TODO : Comment
        private FieldRelationHandler fieldHandler = new FieldRelationHandler();

        // TODO : Comment
        public TestInjectorOnlyFromContainer(OnlyFromContainerInjectable injectable)
            : base(injectable)
        {
        }

        // TODO : Comment
        protected override void InjectAll()
        {
            List<FieldInfo> fields = GetAllFields(mainInjectable.GetType());

            try
            {
                StoreContainerFields(fields.Where(NixInjectFromContainerPredicate));
            }
            catch (NixiAttributeException exception)
            {
                // TODO : Custom Exception ?
                //throw new TestInjectorException(exception.Message);
                throw new NotImplementedException(exception.Message);
            }
        }

        // TODO : Comment
        private void StoreContainerFields(IEnumerable<FieldInfo> containerFields)
        {
            foreach (FieldInfo containerField in containerFields)
            {
                NixInjectFromContainerAttribute injectAttribute = containerField.GetCustomAttribute<NixInjectFromContainerAttribute>();
                injectAttribute.CheckIsValidAndBuildDataFromField(containerField);

                fieldHandler.AddField(new SimpleFieldInfo { FieldInfo = containerField });
            }
        }

        // TODO : Comment
        public T ReadField<T>()
        {
            try
            {
                SimpleFieldInfo field = fieldHandler.GetFieldInfoHandler(typeof(T));
                return (T)field.FieldInfo.GetValue(mainInjectable);
            }
            catch(InjectablesContainerException e)
            {
                throw new TestInjectorOnlyFromContainerException($"Cannot ReadField because {e.Message}", mainInjectable);
            }
        }

        // TODO : Comment
        public T ReadField<T>(string fieldName)
        {
            try
            {
                SimpleFieldInfo field = fieldHandler.GetFieldInfoHandler(typeof(T), fieldName);
                return (T)field.FieldInfo.GetValue(mainInjectable);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjectorOnlyFromContainerException($"Cannot ReadField because {e.Message}", mainInjectable);
            }
        }

        // TODO : Comment
        public void InjectField<T>(T valueToInject)
        {
            try
            {
                SimpleFieldInfo field = fieldHandler.GetFieldInfoHandler(typeof(T));
                field.FieldInfo.SetValue(mainInjectable, valueToInject);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjectorOnlyFromContainerException($"Cannot InjectField because {e.Message}", mainInjectable);
            }            
        }

        // TODO : Comment
        public void InjectField<T>(string fieldName, T valueToInject)
        {
            try
            {
                SimpleFieldInfo field = fieldHandler.GetFieldInfoHandler(typeof(T), fieldName);
                field.FieldInfo.SetValue(mainInjectable, valueToInject);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjectorOnlyFromContainerException($"Cannot InjectField because {e.Message}", mainInjectable);
            }
        }
    }
}