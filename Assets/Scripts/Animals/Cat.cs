using Assets.Scripts.Animals;
using UnityEngine;

public sealed class Cat : MonoBehaviour, ICat
{
    bool isFirstUpdate = true;

    public Food FoodToRetrieve;

    void Awake()
    {
        Debug.Log($"Awake : {name}");
    }

    void Start()
    {
        Debug.Log($"Start : {name}");
    }

    void Update()
    {
        if (isFirstUpdate)
        {
            Debug.Log($"Update : {name}");
            isFirstUpdate = false;
        }
    }
}