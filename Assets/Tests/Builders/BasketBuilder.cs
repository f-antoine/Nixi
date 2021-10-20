using Assets.ScriptExample.ComponentsWithEnumerable;
using Assets.ScriptExample.ComponentsWithEnumerable.BadBasket;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class BasketBuilder
    {
        private Basket basket;

        private BasketBuilder()
        {
            basket = new GameObject("BasketName").AddComponent<Basket>();
        }

        internal static BasketBuilder Create()
        {
            return new BasketBuilder();
        }        

        internal Basket Build()
        {
            return basket;
        }

        internal SimpleBasket BuildSimple()
        {
            return new GameObject("SimpleBasketName").AddComponent<SimpleBasket>();
        }

        internal BasketDualList BuildDualList()
        {
            return new GameObject("BasketDualListName").AddComponent<BasketDualList>();
        }

        internal TonsOfBasket BuildTonsOfBasket()
        {
            return new GameObject("TonsOfBasketName").AddComponent<TonsOfBasket>();
        }

        internal BadBasketNotEnumerable BuildNotEnumerable()
        {
            return new GameObject("BadBasketNotEnumerableName").AddComponent<BadBasketNotEnumerable>();
        }

        internal BadBasketListNotInterfaceNorComponent BuildListNotInterfaceNorComponent()
        {
            return new GameObject("BadBasketListNotInterfaceNorComponentName").AddComponent<BadBasketListNotInterfaceNorComponent>();
        }

        internal BadBasketEnumerableNotInterfaceNorComponent BuildEnumerableNotInterfaceNorComponent()
        {
            return new GameObject("BadBasketEnumerableNotInterfaceNorComponentName").AddComponent<BadBasketEnumerableNotInterfaceNorComponent>();
        }

        internal SimpleBasketComponent BuildSimpleBasketComponent()
        {
            return new GameObject("SimpleBasketComponentName").AddComponent<SimpleBasketComponent>();
        }

        internal DualBasketComponent BuildDualBasketComponent()
        {
            return new GameObject("DualBasketComponentName").AddComponent<DualBasketComponent>();
        }

        internal BasketBuilder WithChildFruit(string name, int weight)
        {
            Fruit newFruit = new GameObject(name).AddComponent<Fruit>();
            newFruit.transform.parent = basket.transform;
            newFruit.ChangeWeight(weight);
            return this;
        }
    }
}
