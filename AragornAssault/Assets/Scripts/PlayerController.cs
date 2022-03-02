using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
    [SerializeField] float controlSpeed;
    [SerializeField] float xRange = 10f;
    [SerializeField] float yRange = 7f;
    // Start is called before the first frame update
    [SerializeField] float pitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float controlYawFactor = 2f;
    [SerializeField] float controlRollFactor = 5f;

    float horizontalThrow;
    float verticalThrow;

    private void OnEnable()
    {
        fire.Enable();
        movement.Enable();
    }

    private void OnDisable()
    {
        fire.Disable();
        movement.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessFiring()
    {
        if (fire.ReadValue<float>() > 0.5f)
        {
            Debug.Log("Im firing");
        }
    }

    private void ProcessTranslation()
    {
        horizontalThrow = movement.ReadValue<Vector2>().x;
        verticalThrow = movement.ReadValue<Vector2>().y;

        float xOffset = horizontalThrow * Time.deltaTime * controlSpeed;
        float rawXpos = transform.localPosition.x + xOffset;
        float clampedXpos = Mathf.Clamp(rawXpos, -xRange, xRange);

        float yOffset = verticalThrow * Time.deltaTime * controlSpeed;
        float rawYpos = transform.localPosition.y + yOffset;
        float clampedYpos = Mathf.Clamp(rawYpos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXpos, clampedYpos, transform.localPosition.z);
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * pitchFactor;
        float pitchDueToControlThrow = +verticalThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * controlYawFactor;
        float roll = horizontalThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}
