using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class Grid : MonoBehaviour
{

    public GameObject player;
    [SerializeField] private List<int> visited;
    private Queue<int> neighborsQueue;
    [SerializeField] private int curLocation;
    int dequeueNode;
    private int?[] parentNode;
    System.Diagnostics.Stopwatch timer;

    public Text destText;


    const int CELL_COUNT = 30;

    [SerializeField] bool[] walls = new bool[CELL_COUNT * CELL_COUNT];
    private bool[] isBlocked = new bool[CELL_COUNT * CELL_COUNT];
    [SerializeField] private bool[] isVisited = new bool[CELL_COUNT * CELL_COUNT];
    [SerializeField] private bool[] isChecked = new bool[CELL_COUNT * CELL_COUNT];
    [SerializeField] private List<int> path;

    // Use this for initialization
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        visited = new List<int>();
        neighborsQueue = new Queue<int>();
        parentNode = new int?[CELL_COUNT * CELL_COUNT];
        path = new List<int>();

        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {
            parentNode[a] = null;
        }

        for (int a = 0; a < CELL_COUNT; a++)
        {
            Debug.DrawLine(new Vector3(a, 0, 0), new Vector3(a, 0, CELL_COUNT), Color.green, Mathf.Infinity);
            Debug.DrawLine(new Vector3(0, 0, a), new Vector3(CELL_COUNT, 0, a), Color.green, Mathf.Infinity);
        }

        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {

            walls[a] = false;
            isBlocked[a] = false;
            isVisited[a] = false;
            isChecked[a] = false;
        }

        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {

                if (Random.Range(0, 10) > 7) walls[a] = true;
            
        }
        WallGenerate(walls);
    }

    private void Init()
    {
        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {
            isBlocked[a] = false;
            isVisited[a] = false;
            isChecked[a] = false;
        }
        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {
            parentNode[a] = null;
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
                    //print(hit.point.y);
                    print(Vector3ToCellNum(loc) + "You're blocked by wall mate.");
                }
                else
                {
                    print("New Start");
                    //print(Vector3ToCellNum(loc));
                    //to prevent infinite loop
                    Init();
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
                walls[Vector3ToCellNum(loc)] = true;
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
            //print("Neighbor" + a + " : " + NeighborCell(curLocation)[a]);
            neighborsQueue.Enqueue(NeighborCell(curLocation)[a]);
            isChecked[NeighborCell(curLocation)[a]] = true;
        }

        isChecked[curLocation] = true;
        isVisited[curLocation] = true;

        BFSSearch(dest);

        //if(BFSSearch(dest) == null)StartCoroutine(BFSSearch(dest));
        //else
        //{
        //    StopCoroutine(BFSSearch(dest));
        //    StartCoroutine(BFSSearch(dest));
        //}

    }

    void BFSSearch(Vector3 destVector)
    {
        while (true)
        {
            if (neighborsQueue.Count > 0)
            {
                dequeueNode = neighborsQueue.Dequeue();

                if (dequeueNode == Vector3ToCellNum(destVector))
                {
                    print("Found! : " + Vector3ToCellNum(destVector));
                    DrawPath(dequeueNode);
                    //StopCoroutine(BFSSearch(destVector));
                    break;
                }

                print("DequeueNode : " + dequeueNode);
                if (!isVisited[dequeueNode] && !walls[dequeueNode])
                {
                    for (int b = 0; b < NeighborCell(dequeueNode).Count; b++)
                    {
                        //print("Neighbor" + b + " : " + NeighborCell(dequeueNode)[b]);
                        if (!isChecked[NeighborCell(dequeueNode)[b]] && !walls[NeighborCell(dequeueNode)[b]])
                        {
                            neighborsQueue.Enqueue(NeighborCell(dequeueNode)[b]);
                            print("Enqueued Data : " + NeighborCell(dequeueNode)[b]);
                            parentNode[NeighborCell(dequeueNode)[b]] = dequeueNode;
                            print("CellNo : " + NeighborCell(dequeueNode)[b] + ", ParentNo : " + parentNode[NeighborCell(dequeueNode)[b]]);
                            isChecked[NeighborCell(dequeueNode)[b]] = true;
                            //LineDraw(dequeueNode, Color.red, 1);
                        }
                        //Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.2f, dequeueNode / CELL_COUNT + 1), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT), Color.red, Mathf.Infinity);
                    }
                    visited.Add(dequeueNode);
                    isVisited[dequeueNode] = true;
                    //LineDraw(dequeueNode, Color.blue, 0);
                    //Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.1f, dequeueNode / CELL_COUNT), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT + 1), Color.blue, Mathf.Infinity);
                }
            }
        }
    }

    void WallGenerate(bool[] _walls)
    {
        for (int a = 0; a < CELL_COUNT * CELL_COUNT; a++)
        {
            if (_walls[a] == true && a != Vector3ToCellNum(player.transform.localPosition))
            {
                GameObject newcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newcube.transform.position = CellNumToVector3(a) + new Vector3(0, 0.5f, 0);
                //newcube.AddComponent<Material>()
            }
        }
    }

    List<int> NeighborCell(int cellNum)
    {
        List<int> neighbors = new List<int>();
        //string[] str = new string[3];

        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 3; b++)
            {
                if (a == 0 && (cellNum / CELL_COUNT) != CELL_COUNT - 1) neighbors.Add(cellNum + CELL_COUNT - 1 + b);
                if (a == 1 && b != 1) neighbors.Add(cellNum - 1 + b);
                if (a == 2 && (cellNum / CELL_COUNT) != 0) neighbors.Add(cellNum - CELL_COUNT - 1 + b);

                if (cellNum % CELL_COUNT == 0)
                {
                    neighbors.Remove(cellNum - 1);
                    neighbors.Remove(cellNum + CELL_COUNT - 1);
                    neighbors.Remove(cellNum - CELL_COUNT - 1);
                }

                if (cellNum % CELL_COUNT == CELL_COUNT - 1)
                {
                    neighbors.Remove(cellNum - 1);
                    neighbors.Remove(cellNum + CELL_COUNT + 1);
                    neighbors.Remove(cellNum - CELL_COUNT + 1);
                }

                //str[a] = str[a] + neighbors[b].ToString() + ", ";
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

    void DrawPath(int childNode)
    {
        int beforeNode = childNode;

        while (beforeNode != Vector3ToCellNum(player.transform.position))
        {
            beforeNode = (int)parentNode[beforeNode];
            LineDraw(beforeNode, Color.red, 1);
            path.Add(beforeNode);
        }

        //if (childNode == Vector3ToCellNum(player.transform.position)) return childNode;
        //else
        //{

        //    return DrawPath((int)parentNode[childNode]);
        //}
    }

    void LineDraw(int cellNum, Color color, int mode)
    {
        if(mode==0)Debug.DrawLine(new Vector3(cellNum % CELL_COUNT, 0.1f, cellNum / CELL_COUNT), new Vector3(cellNum % CELL_COUNT + 1, 0.0f, cellNum / CELL_COUNT + 1), color, Mathf.Infinity);
        else if(mode==1)Debug.DrawLine(new Vector3(cellNum % CELL_COUNT, 0.1f, cellNum / CELL_COUNT+1), new Vector3(cellNum % CELL_COUNT+1, 0.0f, cellNum / CELL_COUNT), color, Mathf.Infinity);
    }

    //IEnumerator BFSSearch(Vector3 destVector)
    //{
    //    while (true)
    //    {
    //        if (neighborsQueue.Count > 0)
    //        {
    //            dequeueNode = neighborsQueue.Dequeue();

    //            if (dequeueNode == Vector3ToCellNum(destVector))
    //            {
    //                print("Found! : " + Vector3ToCellNum(destVector));
    //                DrawPath(dequeueNode);
    //                StopCoroutine(BFSSearch(destVector));
    //                break;
    //            }

    //            print("DequeueNode : " + dequeueNode);
    //            if (!isVisited[dequeueNode] && !walls[dequeueNode])
    //            {
    //                for (int b = 0; b < NeighborCell(dequeueNode).Count; b++)
    //                {
    //                    //print("Neighbor" + b + " : " + NeighborCell(dequeueNode)[b]);
    //                    if (!isChecked[NeighborCell(dequeueNode)[b]] && !walls[NeighborCell(dequeueNode)[b]])
    //                    {
    //                        neighborsQueue.Enqueue(NeighborCell(dequeueNode)[b]);
    //                        print("Enqueued Data : " + NeighborCell(dequeueNode)[b]);
    //                        parentNode[NeighborCell(dequeueNode)[b]] = dequeueNode;
    //                        print("CellNo : " + NeighborCell(dequeueNode)[b] + ", ParentNo : " + parentNode[NeighborCell(dequeueNode)[b]]);
    //                        isChecked[NeighborCell(dequeueNode)[b]] = true;
    //                        //LineDraw(dequeueNode, Color.red, 1);
    //                    }
    //                    //Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.2f, dequeueNode / CELL_COUNT + 1), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT), Color.red, Mathf.Infinity);
    //                }
    //                visited.Add(dequeueNode);
    //                isVisited[dequeueNode] = true;
    //                LineDraw(dequeueNode, Color.blue, 0);
    //                //Debug.DrawLine(new Vector3(dequeueNode % CELL_COUNT, 0.1f, dequeueNode / CELL_COUNT), new Vector3(dequeueNode % CELL_COUNT + 1, 0.0f, dequeueNode / CELL_COUNT + 1), Color.blue, Mathf.Infinity);
    //            }
    //        }
    //        yield return null;//new WaitForSeconds(0.01f);
    //    }
    //}
}