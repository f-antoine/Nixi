using UnityEngine;

namespace ScriptExample.Game
{
    [CreateAssetMenu(fileName = "GameHost", menuName = "ScriptableObjects/GameHost")]
    public sealed class SO_GameHost : ScriptableObject
    {
        public int NbSlot;
    }
}