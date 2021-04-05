using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.MobileJoystick;

public class CamController : MonoBehaviour
{
    public Transform target;

    public Button zoomInBtn;
    public Button zoomOutBtn;

    public Button rotateLeftBtn;
    public Button rotateRightBtn;

    public float SmoothSpeed = 0.5f;
    float turnSmoothVelocity;

    public float zoomSpeed = .5f;
    public float minZoom = 5f;
    public float maxZoom = 10f;

    public Vector3 offset;
    public Vector3 positionOffset;
    public Vector3 lookOffset;

    private Vector3 wantedRotation;

    private float currentZoom = 10f;

    private GameObject cam;

    void Start()
    {
        wantedRotation = transform.localEulerAngles;

        cam = GetComponentInChildren<Camera>().gameObject;

        if (zoomInBtn != null)
            zoomInBtn.onButtonDown += ZoomIn;

        if (zoomOutBtn != null)
            zoomOutBtn.onButtonDown += ZoomOut;

        if (rotateLeftBtn != null)
            rotateLeftBtn.onButtonDown += RotateLeft;

        if (rotateRightBtn != null)
            rotateRightBtn.onButtonDown += RotateRight;

    }

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position, SmoothSpeed);
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, -offset * currentZoom + positionOffset, SmoothSpeed);
        cam.transform.LookAt(target.position + lookOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(wantedRotation), SmoothSpeed);
    }

    public void ZoomIn()
    {
        currentZoom = Mathf.Lerp(currentZoom, currentZoom - zoomSpeed, SmoothSpeed);
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void ZoomOut()
    {
        currentZoom = Mathf.Lerp(currentZoom, currentZoom + zoomSpeed, SmoothSpeed);
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void RotateLeft()
    {
        wantedRotation = transform.eulerAngles + Vector3.up * 45;
    }

    public void RotateRight()
    {
        wantedRotation = transform.eulerAngles - Vector3.up * 45;
    }
}
