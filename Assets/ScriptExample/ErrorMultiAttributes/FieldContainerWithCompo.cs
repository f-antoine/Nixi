using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithCompo : MonoBehaviourInjectable
    {
        [FromContainer]
        [Component]
        public Sorcerer Sorcerer;
    }
}