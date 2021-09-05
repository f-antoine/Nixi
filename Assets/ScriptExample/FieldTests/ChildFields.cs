using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;
using ScriptExample.Containers;

namespace ScriptExample.FieldTests
{
    public class ChildFields : ParentFields
    {
        /// <summary>
        /// Easy access to ITestInterface fields from name for testing simplification purposes
        /// </summary>
        public override ITestInterface GetInterfaceFieldFromName(string fieldName)
        {
            if (fieldName == "privateChildTestInterface") return privateChildTestInterface;
            if (fieldName == "protectedChildTestInterface") return protectedChildTestInterface;
            if (fieldName == "publicChildTestInterface") return publicChildTestInterface;
            return base.GetInterfaceFieldFromName(fieldName);
        }

        /// <summary>
        /// Easy access to Skill MonoBehaviour fields from name for testing simplification purposes
        /// </summary>
        public override Skill GetSkillFieldFromName(string fieldName)
        {
            if (fieldName == "privateChildSkill") return privateChildSkill;
            if (fieldName == "protectedChildSkill") return protectedChildSkill;
            if (fieldName == "publicChildSkill") return publicChildSkill;
            return base.GetSkillFieldFromName(fieldName);
        }

        [NixInject]
        private ITestInterface privateChildTestInterface;

        [NixInject]
        protected ITestInterface protectedChildTestInterface;

        [NixInject]
        public ITestInterface publicChildTestInterface;

        [NixInjectMonoBehaviourFromMethod("privateChildSkill", GameObjectMethod.GetComponentsInChildren)]
        private Skill privateChildSkill;

        [NixInjectMonoBehaviourFromMethod("protectedChildSkill", GameObjectMethod.GetComponentsInChildren)]
        protected Skill protectedChildSkill;

        [NixInjectMonoBehaviourFromMethod("publicChildSkill", GameObjectMethod.GetComponentsInChildren)]
        public Skill publicChildSkill;
    }
}