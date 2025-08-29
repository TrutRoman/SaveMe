using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void Update()
    {
        transform.localPosition += transform.right * Time.deltaTime * -15;
    }
}
