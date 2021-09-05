using System;

namespace Assets.Scripts.Injections
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ComponentInjectAttribute : Attribute
    {
        public string ComponentName { get; private set; }

        public ComponentInjectAttribute(string componentName)
        {
            ComponentName = componentName;
        }
    }
}