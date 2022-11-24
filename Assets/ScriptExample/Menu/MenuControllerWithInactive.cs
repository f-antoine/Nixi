using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Menu
{
    public sealed class MenuControllerWithInactive : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("OptionRoot", "OptionsController", false)]
        public OptionsController OptionsController;

        [NixInjectRootComponent("OptionRoot", "ScreenOptions", false)]
        public ScreenOptions ScreenOptions;
    }
}