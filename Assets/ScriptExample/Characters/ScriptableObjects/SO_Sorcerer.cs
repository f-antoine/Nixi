using UnityEngine;

namespace ScriptExample.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Sorcerer", menuName = "ScriptableObjects/Sorcerer")]
    public sealed class SO_Sorcerer : ScriptableObject
    {
        public string CharaName;
        public int ManaMax;
    }
}
