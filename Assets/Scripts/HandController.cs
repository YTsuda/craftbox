using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    private Rigidbody rb;
    private float hammerSlidePosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Vector3 p = transform.position;
        rb.MovePosition(rb.position + new Vector3(0, 0, hammerSlidePosition));
        if (rb.position.z < -0.8f) {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, -0.8f));
        } else if (rb.position.z > 0.8f){
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, 0.8f));
        } else {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, hammerSlidePosition));
        }
    }

    void OnSlide(InputValue inputValue) {
        print("hogehoge");
        Vector2 v = inputValue.Get<Vector2>();
        print(v);
        float x = v.x;
        hammerSlidePosition = (x - Screen.width / 2) / 3000;
    }
}
