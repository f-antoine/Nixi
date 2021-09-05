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
        [NixInject]
        private IBrokenTestInterface brokenTestInterface;
        public IBrokenTestInterface BrokenTestInterface => brokenTestInterface;

        [NixInject]
        private IBrokenTestInterface brokenTestInterfaceSecond;
        public IBrokenTestInterface BrokenTestInterfaceSecond => brokenTestInterfaceSecond;
    }
}