using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Containers;
using UnityEngine;

namespace ScriptExample.FieldReading
{
    public sealed class FieldsToRead : MonoBehaviourInjectable
    {
        public decimal FirstDecimal;

        [SerializeField]
        public int FirstInt;

        [SerializeField]
        public int SecondInt;

        [NixInjectFromContainer]
        public ITestInterface FirstInterface;

        [NixInjectFromContainer]
        public ITestInterface SecondInterface;

        [NixInjectComponent]
        public Sorcerer Sorcerer;

        [SerializeField]
        public string SingleString = "";

        [NixInjectFromContainer]
        public ISecondTestInterface SingleSecondTestInterface;

        [SerializeField]
        public Fruit SingleFruit;

        [NixInjectComponent]
        public IFruit SingleIFruit;

        [SerializeField]
        public Leprechaun FirstLeprechaun;

        [SerializeField]
        public Leprechaun SecondLeprechaun;

        [SerializeField]
        public ILeprechaun FirstILeprechaun;

        [SerializeField]
        public ILeprechaun SecondILeprechaun;
    }
}
