using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithFieldTestMock : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectTestMock]
        public Sorcerer Sorcerer;
    }
}
