using UnityEngine;

public sealed class Food : MonoBehaviour
{
    bool isFirstUpdate = true;
    public string Name;

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