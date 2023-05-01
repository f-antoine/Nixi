using ScriptExample.ComponentsWithEnumerable;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class BasketBuilder
    {
        private GameObject lastParent;
        private GameObject basketGameObject;

        private BasketBuilder()
        {
            basketGameObject = new GameObject("BasketName");
            lastParent = basketGameObject;
        }

        internal static BasketBuilder Create()
        {
            return new BasketBuilder();
        }

        internal Basket Build()
        {
            return basketGameObject.AddComponent<Basket>();
        }

        internal BasketWithChildrenAndParents BuildBasketWithChildrenAndParents()
        {
            return basketGameObject.AddComponent<BasketWithChildrenAndParents>();
        }

        internal BasketBuilder WithParentFruit(string name, int weight)
        {
            Fruit newFruit = new GameObject(name).AddComponent<Fruit>();
            newFruit.ChangeWeight(weight);
            lastParent.transform.parent = newFruit.transform;
            lastParent = newFruit.gameObject;
            return this;
        }

        internal BasketBuilder WithLocalFruit(int weight)
        {
            Fruit newFruit = basketGameObject.gameObject.AddComponent<Fruit>();
            newFruit.ChangeWeight(weight);
            return this;
        }

        internal BasketBuilder WithChildFruit(string name, int weight)
        {
            Fruit newFruit = new GameObject(name).AddComponent<Fruit>();
            newFruit.transform.parent = basketGameObject.transform;
            newFruit.ChangeWeight(weight);
            return this;
        }
    }
}
