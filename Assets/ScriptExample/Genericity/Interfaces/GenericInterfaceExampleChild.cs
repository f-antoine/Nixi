using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public IGenericInterface<int> ChildGenericityInterface;
    }
}
