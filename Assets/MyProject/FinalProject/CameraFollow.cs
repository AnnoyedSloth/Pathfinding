﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject playerObj;
    Vector3 cameraPos;
    Vector3 ray;
<<<<<<< HEAD
    [SerializeField]bool isPlayerMode;

    private float rotHorizontal;
    private float rotVertical;

    public float upDownRange = 90;
    public float mouseSensitivity = 2.0f;
=======
    bool isPlayerMode;
>>>>>>> be1973aca159802c7552c77483d6b81fb226cc26
	// Use this for initialization
	void Start () {
        isPlayerMode = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlayerMode)
        {
            //transform.position = new Vector3(playerObj.transform.position.x - 3, playerObj.transform.position.y + 3, playerObj.transform.position.z);
            //transform.rotation = Quaternion.Euler(playerObj.transform.rotation.eulerAngles.x + 25, playerObj.transform.rotation.eulerAngles.y, playerObj.transform.rotation.eulerAngles.z);
            //(Vector3(playerObj.transform.rotation.x + 25, playerObj.transform.rotation.y + 90, playerObj.transform.rotation.z);
        }
        else
        {
<<<<<<< HEAD
            //ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            rotHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(0f, rotHorizontal, 0f);

            //rotVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            //rotVertical = Mathf.Clamp(rotVertical, -upDownRange, upDownRange);
            //Camera.main.transform.localRotation = Quaternion.Euler(rotVertical, rotHorizontal, 0f);

            //if (Input.GetKey(KeyCode.W))
            //{
            //    transform.Translate(new Vector3(0, 0, 30) * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    transform.Translate(new Vector3(-30, 0, 0) * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    transform.Translate(new Vector3(0, 0, -30) * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    transform.Translate(new Vector3(30, 0, 0) * Time.deltaTime);
            //}

            //if(Input.mou)

        }
	}

    
=======
            ray = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            //if (Physics.Raycast(ray, Camera.main.transform.forward * 100))
            //{

            //}

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
            }

            //if(Input.mou)

        }

	}
>>>>>>> be1973aca159802c7552c77483d6b81fb226cc26

    public bool CheckPlayerMode()
    {
        return isPlayerMode;
    }
}
