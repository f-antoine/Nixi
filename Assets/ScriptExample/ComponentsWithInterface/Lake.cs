using UnityEngine;

namespace ScriptExample.ComponentsWithInterface
{
    public interface ILakeProperty
    {
        int Depth { get; }
        void DefineDepth(int depth);
    }

    public sealed class Lake : MonoBehaviour, ILakeProperty
    {
        public int Depth { get; private set; }
        public void DefineDepth(int depth)
        {
            Depth = depth;
        }
    }
}