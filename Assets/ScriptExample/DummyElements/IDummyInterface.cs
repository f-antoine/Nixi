using System;

namespace ScriptExample.DummyElements
{
    public interface IDummyInterface
    {
        public int ValueToRetrieve { get; set; }

        public event Action GotTriggered;
        public event Action<int> GotTriggeredWithInt;

        public void Increment();
        public int Increment(int nbIncrement);
        public void Decrement();
        public int Decrement(int nbDecrement);
    }
}