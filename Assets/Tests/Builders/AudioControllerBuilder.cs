using ScriptExample.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.Builders
{
    internal sealed class AudioControllerBuilder
    {
        private GameObject audioController;
        private GameObject lastChild;

        private AudioControllerBuilder()
        {
            audioController = new GameObject();
            lastChild = audioController.gameObject;
        }

        internal static AudioControllerBuilder Create()
        {
            return new AudioControllerBuilder();
        }

        internal AudioController Build()
        {
            audioController.name = "AudioController";
            return audioController.AddComponent<AudioController>();
        }

        internal AudioControllerWithInactive BuildWithInactive()
        {
            audioController.name = "AudioControllerWithInactive";
            return audioController.AddComponent<AudioControllerWithInactive>();
        }

        internal UnderGroundAudioController BuildUnderGround()
        {
            UnderGroundAudioController ugAudio = new GameObject("UnderGroundAudioController").AddComponent<UnderGroundAudioController>();
            ugAudio.transform.SetParent(lastChild.transform);
            return ugAudio;
        }

        internal UnderGroundAudioControllerWithInactive BuildUnderGroundWithInactive()
        {
            UnderGroundAudioControllerWithInactive ugAudio = new GameObject("UnderGroundAudioController").AddComponent<UnderGroundAudioControllerWithInactive>();
            ugAudio.transform.SetParent(lastChild.transform);
            return ugAudio;
        }

        internal AudioControllerBuilder AddEmptyGameObjectLevel()
        {
            GameObject emptyGameObject = new GameObject();
            emptyGameObject.transform.SetParent(lastChild.transform);
            lastChild = emptyGameObject;
            return this;
        }

        internal AudioControllerBuilder AddSliderGameObjectLevel(string name, bool isActive = true)
        {
            Slider slider = new GameObject(name).AddComponent<Slider>();
            slider.transform.SetParent(lastChild.transform);
            slider.gameObject.SetActive(isActive);
            lastChild = slider.gameObject;
            return this;
        }
    }
}