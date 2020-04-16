using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testbox : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = FindObjectOfType<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
