using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public interface IFruit
    {
        public int Weight { get; }
        public string Name { get; }
    }

    public sealed class Fruit : MonoBehaviour, IFruit
    {
        public int Weight { get; private set; }

        public string Name => gameObject.name;

        public void ChangeWeight(int weight)
        {
            Weight = weight;
        }
    }
}