using Nixi.Injections.Attributes;
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
        /// Easy access to Skill Component fields from name for testing simplification purposes
        /// </summary>
        public override Skill GetSkillFieldFromName(string fieldName)
        {
            if (fieldName == "privateChildSkill") return privateChildSkill;
            if (fieldName == "protectedChildSkill") return protectedChildSkill;
            if (fieldName == "publicChildSkill") return publicChildSkill;
            return base.GetSkillFieldFromName(fieldName);
        }

        [NixInjectFromContainer]
        private ITestInterface privateChildTestInterface;

        [NixInjectFromContainer]
        protected ITestInterface protectedChildTestInterface;

        [NixInjectFromContainer]
        public ITestInterface publicChildTestInterface;

        [NixInjectComponentFromChildren("privateChildSkill")]
        private Skill privateChildSkill;

        [NixInjectComponentFromChildren("protectedChildSkill")]
        protected Skill protectedChildSkill;

        [NixInjectComponentFromChildren("publicChildSkill")]
        public Skill publicChildSkill;
    }
}