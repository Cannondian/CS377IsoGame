using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tank hover pad mechanic not currently being used
/*
public class HoverPad : MonoBehaviour
{
    private Rigidbody HoverPadRigidbody;
    private Rigidbody TankRigidbody;

    void Start()
    {
        HoverPadRigidbody = GetComponent<Rigidbody>();
        TankRigidbody = transform.parent.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // We get the rest height on each update in case we want to be able to change it in tank script
        // This is the type of thing that might benefit from an observer setup
        float RestHeight = GetComponentInParent<Tank01Controller>().TankRestHeight;
        float ControlFactor = GetComponentInParent<Tank01Controller>().TankRestHeight;

        // Modify thrust depending on distance from ground using sigmoid inspired function
        // Such that at RestHeight the modifier = 1 and goes to 2 at lower positions, 0 at higher
        // And the ControlFactor determines the slope of the sigmoid (how quickly it varies from 0 to 2)
        float ThrustModifier = 2 / (1 + Mathf.Exp(ControlFactor * (transform.position.y - RestHeight)));
        Vector3 UpwardThrust = new Vector3(0f, ThrustModifier * TankRigidbody.mass/6 * -Physics.gravity.y, 0f);
        HoverPadRigidbody.AddForce(UpwardThrust, ForceMode.Force);
    }
}
*/