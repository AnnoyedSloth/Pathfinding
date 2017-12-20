using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find : MonoBehaviour {

    Transform parent;

	// Use this for initialization
	void Start () {
        parent = GameObject.Find("Parent2/Child").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(parent.position);
	}
}
