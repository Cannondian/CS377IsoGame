using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Modify thrust depending on distance from ground
        float DiffFromRest = transform.position.y - RestHeight;
        float ThrustModifier = 1/(DiffFromRest + RestHeight);
        Vector3 UpwardThrust = new Vector3(0f, ThrustModifier * TankRigidbody.mass/6 * -Physics.gravity.y, 0f);
        HoverPadRigidbody.AddForce(UpwardThrust, ForceMode.Force);
    }
}
