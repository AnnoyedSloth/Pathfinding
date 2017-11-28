using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Grid : MonoBehaviour {

    public GameObject player;

    const int CELL_COUNT = 30;

    bool[,] walls = new bool[CELL_COUNT, CELL_COUNT];

	// Use this for initialization
	void Start ()
    {
        for (int a = 0; a < CELL_COUNT; a++)
        {
            Debug.DrawLine(new Vector3(a, 0, 0), new Vector3(a, 0, CELL_COUNT), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a), new Vector3(CELL_COUNT, 0, a), Color.green, Mathf.Infinity);
        }

        for(int a=0; a< CELL_COUNT; a++)
        {
            for (int b=0; b< CELL_COUNT; b++)
            {
                walls[a, b] = false;
            }
        }

        for(int a=0; a< CELL_COUNT; a++)
        {
            for(int b=0; b< CELL_COUNT; b++)
            {
                if(Random.Range(0,10) > 7) walls[a, b] = true;
            }
        }
        WallGenerate(walls);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Vector3 loc = new Vector3(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));

                //cube.transform.position = loc;
                //cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //print(cube.transform.localPosition);

                if (hit.point.y > player.transform.localPosition.y + 0.3f)
                {
                    //print(hit.point.y);
                    print("You're blocked by wall mate.");
                }
                else
                {
                    //print(hit.point.y);
                    print(CellNum(loc));
                    //for (int a = 0; a < 3; a++)
                    //{
                    //    print(NeighborCell(CellNum(loc))[a]);
                    //}
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Vector3 loc = new Vector3(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(loc.x+0.5f, loc.y + 0.5f, loc.z + 0.5f);
                cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }

    }
    
    void WallGenerate(bool[,] _walls)
    {       

        for (int a=0; a< CELL_COUNT; a++)
        {
            for(int b=0; b<CELL_COUNT; b++)
            {
                if (_walls[a,b] == true)
                {
                    GameObject newcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newcube.transform.position = new Vector3(a+0.5f, 0.5f, b+0.5f);
                }
            }
        }
    }

    string[] NeighborCell(int cellNum)
    {
        int[] neighbors = new int[8];
        string[] str = new string[3];

        for(int a=0; a<3; a++)
        {
            for(int b=0; b<3; b++)
            {
                if (a == 0 && (cellNum / CELL_COUNT) != CELL_COUNT-1) neighbors[b] = cellNum + CELL_COUNT - 1 + b;
                if (a == 1) neighbors[b] = cellNum - 1 + b;
                if (a == 2 && (cellNum / CELL_COUNT) != 0) neighbors[b] = cellNum - CELL_COUNT - 1 + b;
                

                str[a] = str[a] + neighbors[b].ToString() + ", ";
            }
        }
        return str;
    }

    int CellNum(Vector3 location)
    {   
       return (int)location.x + (int)location.z * CELL_COUNT;
    }
}
