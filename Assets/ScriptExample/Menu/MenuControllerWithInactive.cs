using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Menu
{
    public sealed class MenuControllerWithInactive : MonoBehaviourInjectable
    {
        [RootComponent("OptionRoot", "OptionsController", false)]
        public OptionsController OptionsController;

        [RootComponent("OptionRoot", "ScreenOptions", false)]
        public ScreenOptions ScreenOptions;
    }
}