using Nixi.Injections;

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