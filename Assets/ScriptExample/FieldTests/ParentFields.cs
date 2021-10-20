using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;
using ScriptExample.Containers;
using System.Collections.Generic;

namespace ScriptExample.FieldTests
{
    public class ParentFields : MonoBehaviourInjectable
    {
        /// <summary>
        /// Easy access to ITestInterface fields from name for testing simplification purposes
        /// </summary>
        public virtual ITestInterface GetInterfaceFieldFromName(string fieldName)
        {
            if (fieldName == "privateParentTestInterface") return privateParentTestInterface;
            if (fieldName == "protectedParentTestInterface") return protectedParentTestInterface;
            if (fieldName == "publicParentTestInterface") return publicParentTestInterface;
            throw new KeyNotFoundException($"Test cannot be performed because you try to get field with name {fieldName} and it does not exist");
        }

        /// <summary>
        /// Easy access to Skill Component fields from name for testing simplification purposes
        /// </summary>
        public virtual Skill GetSkillFieldFromName(string fieldName)
        {
            if (fieldName == "privateParentSkill") return privateParentSkill;
            if (fieldName == "protectedParentSkill") return protectedParentSkill;
            if (fieldName == "publicParentSkill") return publicParentSkill;
            throw new KeyNotFoundException($"Test cannot be performed because you try to get field with name {fieldName} and it does not exist");
        }

        [NixInjectFromContainer]
        private ITestInterface privateParentTestInterface;

        [NixInjectFromContainer]
        protected ITestInterface protectedParentTestInterface;

        [NixInjectFromContainer]
        public ITestInterface publicParentTestInterface;

        [NixInjectComponentFromMethod("privateParentSkill", GameObjectMethod.GetComponentsInChildren)]
        private Skill privateParentSkill;

        [NixInjectComponentFromMethod("protectedParentSkill", GameObjectMethod.GetComponentsInChildren)]
        protected Skill protectedParentSkill;

        [NixInjectComponentFromMethod("publicParentSkill", GameObjectMethod.GetComponentsInChildren)]
        public Skill publicParentSkill;
    }
}