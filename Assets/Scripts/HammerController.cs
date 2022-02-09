using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerController : MonoBehaviour
{
    private Rigidbody rb;
    HingeJoint hinge;
    JointSpring spring;
    public float beginPos;
    public float shotPos;
    private float hammerSlidePosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        hinge = GetComponent<HingeJoint>();
        spring = hinge.spring;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnStrike(InputValue inputValue)
    {
        Debug.Log("Strike!");
        // Debug.Log(inputValue.isPressed);
        // Debug.Log(inputValue.Get<float>());

        if (inputValue.isPressed)
        {
            spring.targetPosition = beginPos;
            hinge.spring = spring;
        } else {
            spring.targetPosition = shotPos;
            hinge.spring = spring;
        }
    }
}
