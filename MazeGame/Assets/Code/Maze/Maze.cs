using System.Collections.Generic;
using UnityEngine;

public class RoomCell : Cell //classes for handling 3D object in their separate classes
{
    public override GameObject CreateInstance(int mazeWidth)
    {
        m_instancedGameObject = GameObject.Instantiate<GameObject>(m_cellData.m_roomPieces[0]);
        base.CreateInstance(mazeWidth);
        return m_instancedGameObject;
    }
}

public class WallCell : Cell
{
    public override GameObject CreateInstance(int mazeWidth)
    {
        m_instancedGameObject = GameObject.Instantiate<GameObject>(m_cellData.m_wallPieces[0]);
        base.CreateInstance(mazeWidth);
        return m_instancedGameObject;
    }
}

public class Cell
{
    public CellData m_cellData = null;
    public Vector2 m_size = Vector2.one;
    public GameObject m_instancedGameObject = null;
    public int m_xindex = 0;
    public int m_yindex = 0;

    public Cell(){

    }

    public RoomCell ToRoomCell()
    {
        RoomCell cell = new RoomCell();
        cell.m_xindex = this.m_xindex;
        cell.m_yindex = this.m_yindex;
        cell.m_cellData = this.m_cellData;
        cell.m_size = this.m_size;
        return cell;
    }

    private void ConfigureInstance(GameObject instance)
    {
        MeshFilter mf = instance.GetComponentInChildren<MeshFilter>();
        Mesh msh = mf.sharedMesh;
        //TODO woooashhr
    }

    virtual public GameObject CreateInstance(int mazeWidth) //overridable
    {
        ConfigureInstance(m_instancedGameObject);
        return m_instancedGameObject;
    }    
}

public class Maze : MonoBehaviour
{
    private int m_seed = 0;
    private Cell[] m_maze = null;
    private Vector2 m_size = Vector2.zero;
    private int[] m_indices;
    private int m_indicesCount = 0;
    private Vector2[] m_directions =
    {
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(0, 1)        
    };

    public int m_iterations = 1000;
    public CellData m_cellData; //TODO: celldata

    public void GenerateMaze(int w, int h, int s)
    {
        this.m_seed = s;
        InitMaze(w, h);
        IterateMaze();
        CreateInstances();
        Debug.Log("Katos"); //? attach breakpoint to Unity
    }

    private void CreateInstances()
    {
        foreach(Cell cell in m_maze)
        {
            //TODO: wasssasaa
        }
    }


    private bool IsInQueue(Queue<int> lst, int index) //check if it's already in Queue (list)
    {
        if (lst.Contains(index))
        {
            return true;
        }
        return false;
    }

    private bool IsRoomAlreadyFound(int index)
    {
        for (int i = 0; i < m_indices.Length; ++i)
        {
            if (m_indices[i] == index)
            {
                return true;
            }
        }
        return false;
    }

    private bool FindRoom(int index)
    {
        Queue<int> openList = new Queue<int>();
        Queue<int> closedList = new Queue<int>();
        openList.Enqueue(index);
        int c = 0; // counter
        if (index > -1 && index < Mathf.FloorToInt(this.m_size.x * this.m_size.y))
        {
            if (m_maze[index].GetType() == typeof(WallCell))
            {
                return false;
            }
            while (openList.Count != 0)
            {

                int ind = openList.Dequeue();
                if (!IsInQueue(closedList, ind)) //?are we in closed list
                {
                    if (IsRoomAlreadyFound(ind)) //?are we in a room
                    {
                        return false;
                    }
                    closedList.Enqueue(ind);
                    this.m_indices[this.m_indicesCount] = ind;
                    this.m_indicesCount++;
                }
                int x = Mathf.FloorToInt(ind % this.m_size.x);
                int y = Mathf.FloorToInt(ind % this.m_size.y);
                Vector2 xy = new Vector2(x, y);

                for (int i = 0; i < m_directions.Length; i++)
                {
                    Vector2 txy = xy + m_directions[i];
                    int newInd = Mathf.FloorToInt(txy.y * this.m_size.x + txy.x);
                    if (newInd > -1 && newInd < Mathf.FloorToInt(this.m_size.x * this.m_size.y))
                    {
                        if (m_maze[newInd].GetType() != typeof(WallCell))
                        {
                            if (!IsInQueue(closedList, newInd))
                            {
                                if (!IsInQueue(openList, newInd))
                                {
                                    openList.Enqueue(newInd);
                                }
                            }
                        }
                    }
                }

                if(c > 100000)
                {
                    UnityEngine.Debug.Log("FAILSAFE ACTIVATED.");
                    return false;
                }
                c++;
            }
        }
        if (closedList.Count > 0)
        {
            return true;
        }
        return false;
    }

    private void FindRooms(int index)
    {
        Cell c = this.m_maze[index];

        if (c.GetType() != typeof(WallCell)) //no use changing room to a room
        {
            return;
        }
        int w = (int)this.m_size.x; //(int) <-- casting to int
        int indLeft = index - 1;
        int indRight = index + w;
        int indUp = index - 1;
        int indDown = index + w;
        bool lst1 = FindRoom(indLeft);
        bool lst2 = FindRoom(indRight);
        bool lst3 = FindRoom(indUp);
        bool lst4 = FindRoom(indDown);
        int count = (lst1 ? 1 : 0) + (lst2 ? 1 : 0) + ( lst3 ? 1 : 0) + (lst4 ? 1 : 0); //if true return one for each
        if (count == 2)
        {
            this.m_maze[index] = c.ToRoomCell(); //override with new type of room cell
        }

    }


    private void IterateMaze()
    {
        UnityEngine.Random.InitState(m_seed);
        for (int i = 0; i < m_iterations; ++i)
        {
            int index = UnityEngine.Random.Range(0, m_maze.Length);
            FindRooms(index);
        }
    }

    private void InitMaze(int w, int h)
    {
        int c = w * h;
        m_indices = new int[c]; //room indices
        this.m_size = new Vector2(w, h);
        m_maze = new Cell[c];
        int i = 0;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                Cell cell = null;
                int yoe = y % 2;
                int xoe = x % 2;
                if (yoe == 1 && xoe == 1)
                {
                    cell = new RoomCell();
                }
                else
                {
                    cell = new WallCell();
                }
                cell.m_xindex = x;
                cell.m_yindex = y;
                cell.m_cellData = m_cellData;
                m_maze[i] = cell;
                i++;
            }
        }
    }
}
