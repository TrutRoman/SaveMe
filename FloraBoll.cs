using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraBoll : MonoBehaviour
{
    public float speed = 10f;
    float TimeDestroy = 10f;
    bool isRotateArra = true;
    public static int DamageBoll = 10;

    void Start()
    {
        if (Arra.isRotateArra)
        {
            isRotateArra = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            isRotateArra = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void Update()
    {
        if (isRotateArra)
        {
            transform.localPosition += transform.right * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += transform.right * -speed * Time.deltaTime;
        }
        TimeDestroy -= Time.deltaTime;
        if (TimeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player") && !other.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
    }
}
