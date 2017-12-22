using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    public bool ismove = false;
    Transform target;
    Quaternion look;
    Vector3 tarvec;
    Vector3 start;
    Vector3 end;
    // Use this for initialization
	void Start () {
        target = GameObject.Find("Sphere").gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ismove = true;
        }
        if (ismove)
        {
            tarvec = target.position - transform.position;
            tarvec.Normalize();
            look = Quaternion.LookRotation(tarvec);
            //transform.rotation = look;
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, look, Time.deltaTime);
        }
	}
}
