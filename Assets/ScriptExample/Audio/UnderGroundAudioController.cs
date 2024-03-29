﻿using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class UnderGroundAudioController : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("SliderMusic")]
        public Slider musicSlider;

        [NixInjectComponentFromParent("SliderSpatialisation")]
        public Slider spatialisationSlider;
    }
}