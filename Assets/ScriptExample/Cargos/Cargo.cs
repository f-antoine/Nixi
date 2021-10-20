using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptExample.Cargos
{
    public sealed class Cargo : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        private List<Container> firstContainers = new List<Container>();
        public List<Container> FirstContainers => firstContainers;

        [NixInjectComponentList]
        private List<Container> secondContainers = new List<Container>();
        public List<Container> SecondContainers => secondContainers;

        [NixInjectComponent]
        private Transform waterTransform;
        public Transform WaterTransform => waterTransform;
    }
}