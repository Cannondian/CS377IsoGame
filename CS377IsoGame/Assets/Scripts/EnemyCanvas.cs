using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    private Camera camera;

    void Awake()
    {
        camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Align the rotation of the UI to match the rotation of the camera
        transform.rotation = camera.transform.rotation;

        // Optionally, adjust the rotation so that the UI only rotates on the Y-axis
        // This keeps the UI upright but facing the camera
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

}
