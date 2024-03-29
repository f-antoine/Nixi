﻿using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class AudioController : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("SliderMusic")]
        public Slider musicSlider;

        [NixInjectComponentFromChildren("SliderSpatialisation")]
        public Slider spatialisationSlider;
    }
}