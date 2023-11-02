using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCar : MonoBehaviour
{
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3 (0,1,0) * speed * Time.deltaTime);
    }
}