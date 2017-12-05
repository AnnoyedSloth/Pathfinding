using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class Grid : MonoBehaviour
{

    public GameObject player;
    private List<int> visited;
    [SerializeField] private Queue<int> neighborsQueue;
    [SerializeField] private int curLocation;
    int dequeueNode;
    System.Diagnostics.Stopwatch timer;

    public Text destText;


    const int CELL_COUNT = 30;

    bool[,] walls = new bool[CELL_COUNT, CELL_COUNT];
    private bool[,] isBlocked = new bool[CELL_COUNT, CELL_COUNT];
    [SerializeField] private bool[] isVisited = new bool[CELL_COUNT * CELL_COUNT];

    // Use this for initialization
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        visited = new List<int>();
        neighborsQueue = new Queue<int>();

        for (int a = 0; a < CELL_COUNT; a++)
        {
            Debug.DrawLine(new Vector3(a, 0, 0), new Vector3(a, 0, CELL_COUNT), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a), new Vector3(CELL_COUNT, 0, a), Color.green, Mathf.Infinity);
        }

        for (int a = 0; a < CELL_COUNT; a++)
        {
            for (int b = 0; b < CELL_COUNT; b++)
            {
                walls[a, b] = false;
                isBlocked[a, b] = false;
                isVisited[a * 30 + b] = false;
            }
        }

        for (int a = 0; a < CELL_COUNT; a++)
        {
            for (int b = 0; b < CELL_COUNT; b++)
            {
                if (Random.Range(0, 10) > 7) walls[a, b] = true;
            }
        }
        //WallGenerate(walls);
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
                    //print(hit.point.y);
                    print("You're blocked by wall mate.");
                }
                else
                {
                    print("New Start");
                    //print(Vector3ToCellNum(loc));
                    //to prevent infinite loop
                    FindDest(loc);
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
                cube.transform.position = new Vector3(loc.x + 0.5f, loc.y + 0.5f, loc.z + 0.5f);
                cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    void FindDest(Vector3 dest)
    {
        neighborsQueue.Clear();
        //print(hit.point.y);

        curLocation = Vector3ToCellNum(player.transform.localPosition);

        destText.text = "Destination : " + Vector3ToCellNum(dest).ToString();

        print("curLocation : " + curLocation);

        for (int a = 0; a < NeighborCell(curLocation).Count; a++)
        {
            print("Neighbor" + a + " : " + NeighborCell(curLocation)[a]);
            neighborsQueue.Enqueue(NeighborCell(curLocation)[a]);
        }

        for (int a = 0; a < 10000; a++)
        {
            dequeueNode = neighborsQueue.Dequeue();

            if (dequeueNode == Vector3ToCellNum(dest)) { print("Found!"); break; }

            print("DequeueNode : " + dequeueNode);
            for (int b = 0; b < NeighborCell(dequeueNode).Count; b++)
            {
                //print("Neighbor" + b + " : " + NeighborCell(dequeueNode)[b]);
                if (!neighborsQueue.Contains(dequeueNode))
                {
                    neighborsQueue.Enqueue(NeighborCell(dequeueNode)[b]);
                }
                Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.1f, dequeueNode / CELL_COUNT), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT + 1), Color.blue, Mathf.Infinity);
            }

            if (!isVisited[dequeueNode])
            {
                visited.Add(dequeueNode);
                isVisited[dequeueNode] = true;
                //Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.2f, dequeueNode / CELL_COUNT + 1), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT), Color.red, Mathf.Infinity);

            }
        }

    }

    void WallGenerate(bool[,] _walls)
    {
        for (int a = 0; a < CELL_COUNT; a++)
        {
            for (int b = 0; b < CELL_COUNT; b++)
            {
                if (_walls[a, b] == true && Vector3ToCellNum(new Vector3(a, 0, b)) != curLocation)
                {
                    GameObject newcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newcube.transform.position = new Vector3(a + 0.5f, 0.5f, b + 0.5f);
                    //newcube.AddComponent<Material>()
                }
            }
        }
    }

    List<int> NeighborCell(int cellNum)
    {
        List<int> neighbors = new List<int>();
        string[] str = new string[3];

        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 3; b++)
            {
                if (a == 0 && (cellNum / CELL_COUNT) != CELL_COUNT - 1) neighbors.Add(cellNum + CELL_COUNT - 1 + b);
                if (a == 1 && b != 1) neighbors.Add(cellNum - 1 + b);
                if (a == 2 && (cellNum / CELL_COUNT) != 0) neighbors.Add(cellNum - CELL_COUNT - 1 + b);

                str[a] = str[a] + neighbors[b].ToString() + ", ";
            }
        }
        return neighbors;
    }

    int Vector3ToCellNum(Vector3 location)
    {
        return (int)location.x + (int)location.z * CELL_COUNT;
    }

    Vector3 CellNumToVector3(int cellNum)
    {
        return new Vector3(cellNum % CELL_COUNT + 0.5f, 0, (cellNum / CELL_COUNT) + 0.5f);
    }

    Vector3 RayVector(int cellNum)
    {
        return new Vector3(cellNum % CELL_COUNT, 0.1f, cellNum / CELL_COUNT + 0.5f);
    }

}