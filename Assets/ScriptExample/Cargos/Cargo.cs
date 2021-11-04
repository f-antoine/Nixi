using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.Cargos
{
    public sealed class Cargo : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<Container> firstContainers = new List<Container>();
        public List<Container> FirstContainers => firstContainers;

        [NixInjectComponents]
        private List<Container> secondContainers = new List<Container>();
        public List<Container> SecondContainers => secondContainers;

        [NixInjectComponentsFromParent]
        private List<Container> parentContainers = new List<Container>();
        public List<Container> ParentContainers => parentContainers;

        [NixInjectComponentsFromChildren]
        private List<Container> childContainers = new List<Container>();
        public List<Container> ChildContainers => childContainers;

        [NixInjectComponent]
        private Transform waterTransform;
        public Transform WaterTransform => waterTransform;
    }
}