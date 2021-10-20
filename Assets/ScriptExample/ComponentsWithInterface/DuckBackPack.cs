using UnityEngine;

namespace Assets.ScriptExample.ComponentsWithInterface
{
    public interface IDuckObjectContainer
    {
        int NbSlot { get; }
        void UpdateNbSlot(int nbSlot);
    }

    public sealed class DuckBackPack : MonoBehaviour, IDuckObjectContainer
    {
        public int NbSlot { get; private set; }

        public void UpdateNbSlot(int nbSlot)
        {
            NbSlot = nbSlot;
        }
    }
}
