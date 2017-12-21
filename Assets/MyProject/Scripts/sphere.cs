using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere : MonoBehaviour
{
    bool sFlag;
    string name;
    string colname;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(printsTrigger());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        colname = collision.rigidbody.name;
    }

    private void OnTriggerStay(Collider other)
    {
        sFlag = true;
        name = other.name;

    }

    private void OnTriggerExit(Collider other)
    {
        sFlag = false;
    }

    IEnumerator printsTrigger()
    {
        while (true)
        {
            if (sFlag)
            {
                print("Sphere collider on" + name);
                print("My name is " + gameObject.name);
                print(colname);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }



        
}