using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    private Rigidbody rb;
    private float hammerSlidePosition = 0;

    // Handが横に動ける幅
    public float sideLimit = 0.8f;

    // マウスの左右に合わせてどれだけ敏感にHandがSlideするか
    public float slideSensitivity = 500;

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
        if (rb.position.z < -sideLimit) {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, -sideLimit));
        } else if (rb.position.z > sideLimit){
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, sideLimit));
        } else {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y, hammerSlidePosition));
        }
    }

    void OnSlide(InputValue inputValue) {
        print("hogehoge");
        Vector2 v = inputValue.Get<Vector2>();
        print(v);
        float x = v.x;
        hammerSlidePosition = (x - Screen.width / 2) / slideSensitivity;
    }
}
