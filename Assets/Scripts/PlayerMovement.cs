using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody2D playerRigidbody;
    private float maxDistanceFromShip;

    void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // if (PlayerBehaviour.isDead)
        //     return;

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalMovement, verticalMovement);
        movement.Normalize();

        // if (dashEnabled)
        // {
        //     if (Input.GetButton("Dash"))
        //     {
        //         playerRigidbody.AddForce(movement * acceleration * playerRigidbody.mass * 1.5f);
        //     }
        // }

        playerRigidbody.velocity = movement * maxSpeed;

        // playerRigidbody.AddForce(movement * acceleration * playerRigidbody.mass);
        // playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, maxSpeed);

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        }
    }
}