using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Menu
{
    public sealed class MenuController : MonoBehaviourInjectable
    {
        [RootComponent("OptionRoot", "OptionsController")]
        public OptionsController OptionsController;

        [RootComponent("OptionRoot", "ScreenOptions")]
        public ScreenOptions ScreenOptions;
    }
}