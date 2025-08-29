using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject Boulder;
    public GameObject Arrow;
    public bool isBoulder;
    public bool isArrow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBoulder)
            {
                Instantiate(Boulder, transform.position + new Vector3(0, 15, 0), Quaternion.identity);
            }

            if (isArrow)
            {
                Instantiate(Arrow, transform.position + new Vector3(30, 1.5f, 0), Quaternion.identity);
            }
        }
    }
}
