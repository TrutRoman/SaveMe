using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall : MonoBehaviour
{
    public float speed = 10f;
    float TimeDestroy = 20f;
    bool isRotateOrk = true;
    

    void Start()
    {
        if (OrkSh.isRotateOrk)
        {
            isRotateOrk = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            isRotateOrk = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void Update()
    {
        if (isRotateOrk)
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

        if (!other.CompareTag("Enemy") && !other.CompareTag("Ork") && !other.CompareTag("Gul"))
        {
            Destroy(gameObject);
        }
    }
}
