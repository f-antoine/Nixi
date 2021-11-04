using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Menu
{
    public sealed class MenuController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("OptionRoot", "OptionsController")]
        public OptionsController OptionsController;

        [NixInjectRootComponent("OptionRoot", "ScreenOptions")]
        public ScreenOptions ScreenOptions;
    }
}