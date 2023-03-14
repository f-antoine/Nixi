using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Containers.Broken;

namespace ScriptExample.Characters.Broken
{
    /// <summary>
    /// Allow to do test on bad designed code for nixi injection
    /// </summary>
    public class BrokenSorcerer : MonoBehaviourInjectable
    {
        [FromContainer]
        private IBrokenTestInterface brokenTestInterface;
        public IBrokenTestInterface BrokenTestInterface => brokenTestInterface;

        [FromContainer]
        private IBrokenTestInterface brokenTestInterfaceSecond;
        public IBrokenTestInterface BrokenTestInterfaceSecond => brokenTestInterfaceSecond;
    }
}