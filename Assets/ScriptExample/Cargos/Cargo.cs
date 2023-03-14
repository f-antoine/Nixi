using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.Cargos
{
    public sealed class Cargo : MonoBehaviourInjectable
    {
        [Components]
        private List<Container> firstContainers = new List<Container>();
        public List<Container> FirstContainers => firstContainers;

        [Components]
        private List<Container> secondContainers = new List<Container>();
        public List<Container> SecondContainers => secondContainers;

        [ComponentsFromParents]
        private List<Container> parentContainers = new List<Container>();
        public List<Container> ParentContainers => parentContainers;

        [ComponentsFromChildren]
        private List<Container> childContainers = new List<Container>();
        public List<Container> ChildContainers => childContainers;

        [Component]
        private Transform waterTransform;
        public Transform WaterTransform => waterTransform;
    }
}