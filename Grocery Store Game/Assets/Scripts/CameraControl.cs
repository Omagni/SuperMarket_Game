using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraControl : MonoBehaviour
{
    [SerializeField] public float control_zoom = 100f; // The desired value for the "Assets Pixel Per Unit" setting

    void Update()
    {
        // Get a reference to the PixelPerfectCamera component on the GameObject
        PixelPerfectCamera pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        if (Input.mouseScrollDelta.y > 0)
            if (control_zoom < 400)
                control_zoom += 15f;
        if (Input.mouseScrollDelta.y < 0) {
            if (control_zoom > 100)
                control_zoom -= 15f;

        }
                // Update the "Assets Pixel Per Unit" value
                pixelPerfectCamera.assetsPPU = (int)control_zoom;
    }




}

