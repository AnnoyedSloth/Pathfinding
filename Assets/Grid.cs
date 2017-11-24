using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Grid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int a = 0; a < 100; a++)
        {
            Debug.DrawLine(new Vector3(a, 0, 0), new Vector3(a, 0, 100), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a), new Vector3(100, 0, a), Color.green, Mathf.Infinity);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 loc = new Vector3(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));

                cube.transform.position = loc;
                cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                print(cube.transform.localPosition);
                print(CellNum(loc));

            }
        }
    }

    int CellNum(Vector3 location)
    {   
       return (int)location.x + (int)location.z * 100;
    }
}
