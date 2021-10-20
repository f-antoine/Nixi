using Nixi.Injections;
using System.Collections.Generic;
using System.Reflection;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Contain all Component fields and Non-Component fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjecter
    /// </summary>
    internal sealed class MonoBehaviourInjectableData
    {
        /// <summary>
        /// Instance on which all the fields are fields and usable from componentWithFieldInstantiated and nonComponentFields
        /// </summary>
        internal MonoBehaviourInjectable Instance { get; set; }

        /// <summary>
        /// Name of the instance, can help for specials operation like root GameObjects (NixInjectComponentFromMethodRootAttribute)
        /// </summary>
        internal string InstanceName { get; set; }

        /// <summary>
        /// Non-Component fields to inject manually with mocks
        /// </summary>
        internal List<FieldInfo> nonComponentFields = new List<FieldInfo>();

        /// <summary>
        /// All associations of GameObjects with theirs fieldInfo, it reduce the number of type resolution operations
        /// </summary>
        internal List<GameObjectWithFieldInfo> componentWithFieldInstantiated = new List<GameObjectWithFieldInfo>();        
    }
}