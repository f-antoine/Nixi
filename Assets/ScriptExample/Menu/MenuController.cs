using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;

namespace Assets.ScriptExample.Menu
{
    public sealed class MenuController : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("OptionRoot", "OptionsController")]
        public OptionsController OptionsController;

        [NixInjectRootComponent("OptionRoot", "ScreenOptions")]
        public ScreenOptions ScreenOptions;
    }
}