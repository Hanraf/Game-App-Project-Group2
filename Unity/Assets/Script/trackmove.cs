using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackmove : MonoBehaviour
{
    public float speed;
    Vector2 offset; // Perbaikan: 'vector2' diganti dengan 'Vector2'

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(0, Time.time * speed); // Perbaikan: 'vector2' diganti dengan 'Vector2', 'time' diganti dengan 'Time'
        GetComponent<Renderer>().material.mainTextureOffset = offset; // Perbaikan: 'material.mainTextureoffset' diganti dengan 'material.mainTextureOffset'
    }
}
