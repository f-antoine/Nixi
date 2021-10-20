using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldTestMockWithCompo : MonoBehaviourInjectable
    {
        [NixInjectTestMock]
        [NixInjectComponent]
        public Sorcerer Sorcerer;
    }
}