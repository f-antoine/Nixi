using UnityEngine;

namespace ScriptExample.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InventoryBag", menuName = "ScriptableObjects/InventoryBag")]
    public sealed class SO_InventoryBag : ScriptableObject
    {
        public string BagName;
        public int NbSlot;
    }
}