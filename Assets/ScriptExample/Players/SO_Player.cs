using UnityEngine;

namespace ScriptExample.Players
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
    public sealed class SO_Player : ScriptableObject
    {
        public string Pseudo;
    }
}