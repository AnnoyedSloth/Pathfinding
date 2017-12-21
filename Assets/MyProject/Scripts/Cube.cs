using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    Transform Dest;
    Camera Cmr;

    float speed = 3f;
    float startTime;
    float journeyLength;

    private Vector3 velocity = Vector3.zero;
    Quaternion targetRot;

    // Use this for initialization
    void Start () {
        Dest = GameObject.Find("Destination").transform.GetComponent<Transform>();
        Cmr = GameObject.Find("Main Camera").transform.GetComponent<Camera>();

        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, Dest.position);

        targetRot = Quaternion.EulerRotation(-60, 55, 5);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Dest.position = new Vector3(-3, 0.5f, 0);
            startTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.localPosition = Vector3.Lerp(transform.position, Dest.localPosition, Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.C))
        {
            float fracJourney = (Time.time - startTime) * speed / journeyLength;

            transform.position = Vector3.Lerp(transform.position, Dest.position, fracJourney);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = Vector3.SmoothDamp(transform.position, Dest.position, ref velocity, speed);
            Cmr.transform.eulerAngles = Vector3.SmoothDamp(Cmr.transform.eulerAngles, targetRot.eulerAngles, ref velocity, 3.0f);
        }

    }

    IEnumerator TimePrint()
    {
        while (true)
        {
            //txt.text = (elapsedTime - pressedTime).ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

}
