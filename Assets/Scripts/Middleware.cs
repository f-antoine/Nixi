using UnityEngine;

public class Middleware : MonoBehaviour
{
    bool isFirstUpdate = true;

    void Awake()
    {
        Debug.Log($"Awake : {name}");

        MonoBehaviour[] allGos = FindObjectsOfType<MonoBehaviour>();
        int a = 0;
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