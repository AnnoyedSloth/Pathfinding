using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{

    enum isWalkable { Walkable, Unwalkable };

    public GameObject player;

    const int GRID_NUM = 1000;
    const int GRID_SIZE = 1;
    int[] cellNum;
    int[] costH;
    int[] costG;
    int[] costF;
    int[] parent;
    [SerializeField] List<int> openList;
    [SerializeField] List<int> closedList;
    bool[] isClosedNode;
    Vector3 depart;
    Vector3 dest;

    static Vector3[] corners = { new Vector3(1, 0, 0), new Vector3(0, 0, 1) };

    isWalkable[] world = null;

    // Use this for initialization
    void Start()
    {
        costH = new int[GRID_NUM * GRID_NUM];
        costG = new int[GRID_NUM * GRID_NUM];
        costF = new int[GRID_NUM * GRID_NUM];
        parent = new int[GRID_NUM * GRID_NUM];
        openList = new List<int>();
        closedList = new List<int>();
        isClosedNode = new bool[GRID_NUM * GRID_NUM];

        world = new isWalkable[GRID_NUM * GRID_NUM];

        depart = new Vector3();
        dest = new Vector3();

        //SettingUp;
        for (int a = 0; a < GRID_NUM + 1; a++)
        {
            Debug.DrawLine(new Vector3(a * GRID_SIZE, 0, 0), new Vector3(a * GRID_SIZE, 0, GRID_NUM * GRID_SIZE), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a * GRID_SIZE), new Vector3(GRID_NUM * GRID_SIZE, 0, a * GRID_SIZE), Color.green, Mathf.Infinity);
        }
        for (int a = 0; a < GRID_NUM * GRID_NUM; a++)
        {
            world[a] = isWalkable.Walkable;
            isClosedNode[a] = false;
            costH[a] = 0;
            costG[a] = 0;
            costF[a] = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Vector3 loc = new Vector3(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));

                //cube.transform.position = loc;
                //cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //print(cube.transform.localPosition);

                if (hit.point.y > player.transform.localPosition.y + 0.3f)
                {
                    print(hit.point.y);
                    print(Vector2Grid(loc) + "You're blocked by wall mate.");
                }
                else
                {
                    depart = player.transform.position;
                    dest = loc;
                    print(Vector2Grid(loc));

                    ClearValues();

                    PathFind();
                    drawRect(Vector2Grid(loc), Color.red);
                    //FindNeighbor(Vector2Grid(player.transform.position));
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

                world[Vector2Grid(loc)] = isWalkable.Unwalkable;
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.GetComponent<MeshRenderer>().material.color = Color.yellow;
                wall.transform.position = locateToCenter(loc);
            }
        }
    }

    Vector3 locateToCenter(Vector3 pos)
    {
        return pos + new Vector3(0.5f, 0, 0.5f);
    }

    void ClearValues()
    {
        for (int a = 0; a < GRID_NUM * GRID_NUM; a++)
        {
            isClosedNode[a] = false;
            costH[a] = 0;
            costG[a] = 0;
            costF[a] = 0;
        }
        openList.Clear();
        closedList.Clear();
    }

    void PathFind()
    {
        int[] neighborArr = FindNeighbor(Vector2Grid(player.transform.position));

        for (int a = 0; a < 10; a++)
        {
            //StartCoroutine(FindingPath());
            FindNeighbor(FindLowestF());
            OpenListUpdate();
        }
    }

    int FindLowestF()
    {
        int temp = openList[0];
        foreach (var mylist in openList)
        {
            if (costF[temp] > costF[mylist])
            {
                if (costF[temp] == costF[mylist])
                {
                    if (costG[temp] > costG[mylist]) temp = mylist;
                }
                else
                {
                    temp = mylist;
                }
            }
        }

        closedList.Add(temp);
        isClosedNode[temp] = true;
        //print("Pulled Node : " + temp + ", Cost : " + costF[temp]);

        //drawRect(temp, Color.grey, Mathf.Infinity);
        Debug.DrawLine(Grid2Vector(temp), Grid2Vector(temp) + Vector3.right + Vector3.forward, Color.black, Mathf.Infinity);
        openList.Remove(temp);
        //print("Lowest F is : " + temp + ", " + costF[temp]);

        return temp;
    }

    int[] FindNeighbor(int cellNum)
    {
        List<int> neighbors = new List<int>() { -1, 1, -GRID_NUM, GRID_NUM, -GRID_NUM - 1, -GRID_NUM + 1, GRID_NUM - 1, GRID_NUM + 1 };

        //neighbors.Add();

        if (cellNum % GRID_NUM == 0) neighbors.RemoveAll((num) => { return num == -1 || num == -1 - GRID_NUM || num == -1 + GRID_NUM; });
        if (cellNum % GRID_NUM == GRID_NUM - 1) neighbors.RemoveAll((num) => { return num == 1 || num == 1 - GRID_NUM || num == 1 + GRID_NUM; });
        if (cellNum / GRID_NUM == 0) neighbors.RemoveAll((num) => { return num == -GRID_NUM || num == -1 - GRID_NUM || num == -GRID_NUM + 1; });
        if (cellNum / GRID_NUM == GRID_NUM - 1) neighbors.RemoveAll((num) => { return num == GRID_NUM || num == GRID_NUM - 1 || num == GRID_NUM + 1; });


        for (int i = 0; i < neighbors.Count;)
        {

            neighbors[i] += cellNum;
            parent[neighbors[i]] = cellNum;
            if (!openList.Contains(neighbors[i]) && isClosedNode[neighbors[i]] == false) openList.Add(neighbors[i]);
            costG[neighbors[i]] = GetDistance(depart, Grid2Vector(neighbors[i])); // Store distance between Departing position and Current position
            costH[neighbors[i]] = GetDistance(Grid2Vector(neighbors[i]), dest); // Store distance between Current node's position and Destionation's position
            costF[neighbors[i]] = costG[neighbors[i]] + costH[neighbors[i]];

            //print("Cell : " + neighbors[i] + ", Parent : " + parent[neighbors[i]]);
            //print(neighbors[i] + "\ncostG : " + costG[neighbors[i]] + "\ncostH : " + costH[neighbors[i]] + "\ncostF : " + costF[neighbors[i]]);
            drawRect(neighbors[i], Color.yellow, Mathf.Infinity);

            if (neighbors[i] < 0 || neighbors[i] >= GRID_NUM * GRID_NUM || world[neighbors[i]] == isWalkable.Unwalkable) neighbors.RemoveAt(i);
            else i++;
        }
        //for (int a = 0; a < neighbors.Count; a++)
        //{
            //print(neighbors[a] + "\ncostF : " + costF[neighbors[a]] + "\ncostG : " + costG[neighbors[a]]);
        //}
        //foreach (var neighbor in neighbors)
        //{
        //}
        return neighbors.ToArray();
    }

    void OpenListUpdate()
    {
        for (int a = 0; a < openList.Count; a++)
        {
            costG[openList[a]] = GetDistance(depart, Grid2Vector(openList[a])); // Store distance between Departing position and Current position
            costH[openList[a]] = GetDistance(Grid2Vector(openList[a]), dest); // Store distance between Current node's position and Destionation's position
            costF[openList[a]] = costG[openList[a]] + costH[openList[a]];
        }
    }

    static int GetDistance(Vector3 from, Vector3 to)
    {
        int dx = (int)Mathf.Abs(to.x - from.x);
        int dz = (int)Mathf.Abs(to.z - from.z);

        int min = Mathf.Min(dx, dz);
        int max = Mathf.Max(dx, dz);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return 14 * diagonalSteps + 10 * straightSteps;
    }

    static int Vector2Grid(Vector3 vecLoc)
    {
        return (int)vecLoc.x / GRID_SIZE + (int)(vecLoc.z / GRID_SIZE) * GRID_NUM;
    }

    static Vector3 Grid2Vector(int gridNum)
    {
        return new Vector3((gridNum * GRID_SIZE) % GRID_NUM, 0f, gridNum * GRID_SIZE / GRID_NUM);
    }

    void drawRect(int cellno, Color c, float duration = 10000.0f)
    {
        Vector3 correction = new Vector3(0, 0f, 0);
        Vector3 lb = Grid2Vector(cellno) + correction;

        Debug.DrawLine(lb, lb + corners[0], c, duration);
        Debug.DrawLine(lb, lb + corners[1], c, duration);
        Vector3 rt = lb + corners[0] + corners[1] + correction;
        Debug.DrawLine(rt, rt - corners[0], c, duration);
        Debug.DrawLine(rt, rt - corners[1], c, duration);
    }

    IEnumerator FindingPath()
    {
        int a = 0;
        while (a < 500)
        {
            FindNeighbor(FindLowestF());
            OpenListUpdate();
            a++;
            yield return new WaitForSeconds(1f);
        }
    }
}
