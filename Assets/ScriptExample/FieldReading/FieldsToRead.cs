using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
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

        [FromContainer]
        public ITestInterface FirstInterface;

        [FromContainer]
        public ITestInterface SecondInterface;

        [Component]
        public Sorcerer Sorcerer;

        [SerializeField]
        public string SingleString = "";

        [FromContainer]
        public ISecondTestInterface SingleSecondTestInterface;

        [SerializeField]
        public Fruit SingleFruit;

        [Component]
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
