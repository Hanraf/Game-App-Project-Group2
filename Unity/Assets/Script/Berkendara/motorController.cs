using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motorController : MonoBehaviour
{
    public float motorSpeed;
    public float maxPos = 5.401854f;
    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position.x += Input.GetAxis ("Horizontal") * motorSpeed * Time.deltaTime;
        position.x = Mathf.Clamp (position.x, -5.401854f, 5.401854f);
        transform.position = position;
    }

    void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Enemy Car")
        {
            Destroy (gameObject);
        }
    }
}
