using Assets.Scripts.Injections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MonoBehaviourInjectable : MonoBehaviour
    {
        // TODO : Delete
        [SerializeField]
        [ComponentInject("CopyCatInjection")]
        protected Cat NeedToBeInjected;

        protected virtual void Awake()
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // TODO : Voir si on gère héritage sur private
            //FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Concat(GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)).ToArray();

            foreach (FieldInfo field in fields)
            {
                IEnumerable<ComponentInjectAttribute> injectedAttributes = field.GetCustomAttributes<ComponentInjectAttribute>();
                if (!injectedAttributes.Any()) continue;

                ComponentInjectAttribute attributeFound = injectedAttributes.Single();

                Component componentToTranspose = ComponentToRetrieveInChildren(field.FieldType, attributeFound.ComponentName);
                field.SetValue(this, componentToTranspose);
            }
        }

        private Component ComponentToRetrieveInChildren(Type typeToFind, string nameToFind)
        {
            Component[] component = GetComponentsInChildren(typeToFind);

            if (!component.Any())
                throw new InjectionException(typeToFind);

            return GetAndCheckExactlyOneComponentMatch(typeToFind, nameToFind, component);
        }

        private static Component GetAndCheckExactlyOneComponentMatch(Type typeToFind, string nameToFind, Component[] component)
        {
            IEnumerable<Component> componentsWithName = component.Where(x => x.name == nameToFind);

            if (!componentsWithName.Any())
                throw new InjectionException(typeToFind, nameToFind);

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new InjectionException(typeToFind, nameToFind, nbFound);

            return componentsWithName.Single();
        }
    }
}
