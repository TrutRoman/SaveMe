using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDestroy : MonoBehaviour
{
    float TimeDestroy = 5f;
    void Update()
    {
        TimeDestroy -= Time.deltaTime;
        if (TimeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
