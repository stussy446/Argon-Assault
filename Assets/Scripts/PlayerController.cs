using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] InputAction movement; // handles wasd for movement
    [SerializeField] float controlSpeed = 10f; // tunes how fast ship moves
    [SerializeField] float xRange = 5f;
    [SerializeField] float topYRange = 5f;
    [SerializeField] float bottomYRange = 5f;

    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2.5f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -10f;


    float xThrow;
    float yThrow;


    void OnEnable()
    {
        movement.Enable();
    }


    void OnDisable()
    {
        movement.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();

    }


    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float rollDueToControlThrow = xThrow * controlRollFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = yawDueToPosition;
        float roll = rollDueToControlThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }


    void ProcessTranslation()
    {
        // sets the value of x and y movement based on player input 
         xThrow = movement.ReadValue<Vector2>().x;
         yThrow = movement.ReadValue<Vector2>().y;

        // gets objects original localPosition
        transform.localPosition = new Vector3
           (transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z);

        // calculates the amount the object will move horizontally and vertically
        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float yOffset = yThrow * Time.deltaTime * controlSpeed;

        // calculates objects new position
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, bottomYRange, topYRange);


        // moves object to the posiiton
        transform.localPosition = new Vector3
          (clampedXPos,
           clampedYPos,
           transform.localPosition.z);
    }

}
