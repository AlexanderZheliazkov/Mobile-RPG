using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollesion : MonoBehaviour {

    public float minDistance = 1f;
    public float maxDistance = 4f;
    public float smooth = 10f;
    Vector3 Dir;
    public Vector3 DirAjusted;
    public float distance;

	// Use this for initialization
	void Awake () {
        Dir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 desiredCamPosition = transform.parent.TransformPoint(Dir * maxDistance);
        RaycastHit hit;
        if(Physics.Linecast(transform.parent.position,desiredCamPosition,out hit))
        {
            distance = Mathf.Clamp((hit.distance*0.9f), minDistance, maxDistance);

        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, Dir * distance, Time.deltaTime * smooth);
	}
}
