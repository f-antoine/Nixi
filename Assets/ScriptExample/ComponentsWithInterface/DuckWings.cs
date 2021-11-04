using UnityEngine;

namespace ScriptExample.ComponentsWithInterface
{
    public interface IFlyBehavior
    {
        int Height { get; }
        void Fly(int height);
    }

    public sealed class DuckWings : MonoBehaviour, IFlyBehavior
    {
        public int Height { get; private set; }

        public void Fly(int height)
        {
            Height = height;
        }
    }
}