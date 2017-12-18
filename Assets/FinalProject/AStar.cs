using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    const int GRID_NUM = 500;
    const int GRID_SIZE = 2;
    int[] cellNum;
    int[] costH;
    int[] costG;
    int[] costF;
    List<int> openList;
    List<int> closedList;

	// Use this for initialization
	void Start ()
    {
        costH = new int[GRID_NUM * GRID_NUM];
        costG = new int[GRID_NUM * GRID_NUM];
        costF = new int[GRID_NUM * GRID_NUM];
        openList = new List<int>();
        closedList = new List<int>();

        //SettingUp;
        for (int a = 0; a < GRID_NUM+1; a++)
        {
            Debug.DrawLine(new Vector3(a* GRID_SIZE, 0, 0), new Vector3(a * GRID_SIZE, 0, GRID_NUM * GRID_SIZE), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a * GRID_SIZE), new Vector3(GRID_NUM * GRID_SIZE, 0, a * GRID_SIZE), Color.green, Mathf.Infinity);
        }

        for(int a=0; a<GRID_NUM * GRID_NUM; a++)
        {
            costH[a] = 0;
            costG[a] = 0;
            costF[a] = 0;
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Distance : " + GetDistance(new Vector3(1, 0, 3), new Vector3(2, 0, 7)));
        }
	}

    int[] findNeighbor(int cellNum)
    {
        List<int> neighbors = new List<int>();

        //neighbors.Add();

        if (cellNum % GRID_NUM == 0) neighbors.RemoveAll((num) => { return num == -1 || num == -1 - GRID_NUM || num == -1 + GRID_NUM; });
        if (cellNum % GRID_NUM == GRID_NUM - 1) neighbors.RemoveAll((num) => { return num == 1 || num == 1 - GRID_NUM || num == 1 + GRID_NUM; });
        if (cellNum / GRID_NUM == 0) neighbors.RemoveAll((num) => { return num == -GRID_NUM || num == -1 - GRID_NUM || num == -GRID_NUM + 1; });
        if (cellNum / GRID_NUM == GRID_NUM - 1) neighbors.RemoveAll((num) => { return num == GRID_NUM || num == GRID_NUM - 1 || num == GRID_NUM + 1; });

        for(int i=0; i < neighbors.Count;)
        {
            neighbors[i] += cellNum;
            if (neighbors[i] < 0 || neighbors[i] >= GRID_NUM * GRID_NUM || isWall[neighbors[i]] == true) neighbors.RemoveAt(i);
            else i++;
        }



        return neighbors.ToArray();
    }

    static int GetDistance(Vector3 from, Vector3 to)
    {
        int dx = (int)Mathf.Abs(to.x - from.x);
        int dz = (int)Mathf.Abs(to.z - from.z);

        int min = Mathf.Min(dx, dz);
        int max = Mathf.Max(dx, dz);

        int diagonalSteps = min;
        int straightSteps = max - min;

        //print("dx, dz = " + dx + ", " + dz);
        //print("min, max = " + min + ", " + max);
        //print("diagonal, straight = " + diagonalSteps + ", " + straightSteps);

        return 14 * diagonalSteps + 10 * straightSteps;
    }

    static int Vector2Grid(Vector3 vecLoc)
    {
        return (int)vecLoc.x + (int)vecLoc.z * GRID_NUM;
    }

    static Vector3 Grid2Vector(float gridName)
    {
        return new Vector3(gridName % GRID_NUM, 0.5f, gridName /GRID_NUM);
    }
}
