using UnityEngine;

namespace Assets.ScriptExample.FieldReading
{
    public interface ILeprechaun
    {
        public int AngerLevel { get; }

        void ChangeAngerLevel(int angerLevel);
    }

    public sealed class Leprechaun : MonoBehaviour, ILeprechaun
    {
        public int AngerLevel { get; private set; }

        public void ChangeAngerLevel(int angerLevel)
        {
            AngerLevel = angerLevel;
        }
    }
}