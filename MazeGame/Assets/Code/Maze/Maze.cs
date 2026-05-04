using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MazeGame.Maze
{
    public class RoomCell : Cell //classes for handling 3D object in their separate classes
    {
        public override GameObject CreateInstance()
        {
            m_instancedGameObject = GameObject.Instantiate<GameObject>(m_cellData.m_roomPieces[0]);
            base.CreateInstance();
            return m_instancedGameObject;
        }
    }

    public class WallCell : Cell
    {
        public override GameObject CreateInstance()
        {
            m_instancedGameObject = GameObject.Instantiate<GameObject>(m_cellData.m_wallPieces[0]);
            base.CreateInstance();
            return m_instancedGameObject;
        }
    }

    public class ExitCell : Cell
    {
        public override GameObject CreateInstance()
        {
            m_instancedGameObject = GameObject.Instantiate<GameObject>(m_cellData.m_exitPiece);
            CellComponent cc = m_instancedGameObject.AddComponent<CellComponent>();
            cc.m_floor = true;
            // cc.m_index = y.index * mazeWidth + m_xindex;
            //TODO: fix CellComponent adding
            base.CreateInstance();
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
        public bool m_key = false;

        public Cell()
        {

        }

        public RoomCell ToRoomCell()
        {
            RoomCell cell = new RoomCell();
            cell.m_xindex = this.m_xindex;
            cell.m_yindex = this.m_yindex;
            cell.m_cellData = this.m_cellData;
            cell.m_size = this.m_size;
            cell.m_key = this.m_key;
            return cell;
        }

        public ExitCell ToExitCell()
        {
            ExitCell cell = new ExitCell();
            cell.m_xindex = this.m_xindex;
            cell.m_yindex = this.m_yindex;
            cell.m_cellData = this.m_cellData;
            cell.m_size = this.m_size;
            cell.m_key = this.m_key;
            return cell;
        }

        private void ConfigureInstance(GameObject instance, bool isKey = false)
        {
            MeshFilter mf = instance.GetComponentInChildren<MeshFilter>();
            Mesh msh = mf.sharedMesh;
            if (!isKey)
            {
                this.m_size = new Vector2(msh.bounds.size.x, msh.bounds.size.y);
            }

            instance.transform.position = new Vector3(
                m_xindex * m_size.x,
                0f,
                m_yindex * m_size.y);
        }

        virtual public GameObject CreateInstance() //overridable
        {
            ConfigureInstance(m_instancedGameObject);
            if (this.m_key)
            {
                GameObject go = GameObject.Instantiate<GameObject>(m_cellData.m_key);
                ConfigureInstance(go, isKey: true);
            }
            return m_instancedGameObject;
        }
    }

    public class Maze : MonoBehaviour
    {
        private int m_seed = 0;
        private int m_iterations = 2000; //TODO: Make sure every room is connected.
        public Cell[] m_maze = null;
        private Vector2 m_size = Vector2.zero; //Vector2 is a struct that needs to have values
        private int[] m_indices;
        public int m_indicesCount = 0;
        private Vector2[] m_directions =
        {
        new Vector2(-1, 0),
        new Vector2(0, -1),
        new Vector2(1, 0),
        new Vector2(0, 1)
    };

        public CellData m_cellData = null;

        public void GenerateMaze(int w, int h, int s, int i)
        {
            this.m_seed = s;
            this.m_iterations = i;
            InitMaze(w, h);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IterateMaze();
            sw.Stop();
            CreateExit();
            CreateKey();
            CreateInstances(); //TODO: instantiate prefabs?
            UnityEngine.Debug.Log(string.Format("Maze generated in: {0}ms", sw.ElapsedMilliseconds));
        }

        private void CreateExit()
        {
            int c = 0;
            UnityEngine.Random.InitState(m_seed);
            while (true)
            {
                int r = UnityEngine.Random.Range(0, m_maze.Length);
                if (m_maze[r].GetType() != typeof(WallCell))
                {
                    m_maze[r] = m_maze[r].ToExitCell(); //?
                    break;
                }
                if (c > 100)
                {
                    break;
                }
                c++;
            }
        }

        private void CreateKey()
        {
            int c = 0;
            UnityEngine.Random.InitState(m_seed);
            while (true)
            {
                int r = UnityEngine.Random.Range(0, m_maze.Length);
                if (m_maze[r].GetType() != typeof(WallCell))
                {
                    if (m_maze[r].GetType() != typeof(ExitCell))
                    {
                        m_maze[r].m_key = true;
                        break;
                    }

                }
                if (c > 100)
                {
                    break;
                }
                c++;
            }
        }

        private void CreateInstances()
        {
            foreach (Cell cell in m_maze)
            {
                cell.CreateInstance();
                cell.m_instancedGameObject.transform.parent = this.transform;
            }
        }


        private bool IsInQueue(Queue<int> lst, int index) //check if it's already in Queue 
        {
            if (lst.Contains(index))
            {
                return true;
            }
            return false;
        }

        private bool IsRoomAlreadyFound(int index)
        {
            for (int i = 0; i < m_indicesCount; ++i)
            {
                if (m_indices[i] == index)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector3 GetRandomCell(int seed)
        {
            UnityEngine.Random.InitState(seed);
            int c = 0;
            while (c < 100)
            {
                int r = UnityEngine.Random.Range(0, m_indicesCount);
                if (m_maze[r].GetType() == typeof(RoomCell))
                {
                    return this.m_maze[r].m_instancedGameObject.transform.position;
                }
                c++;
            }
            return Vector3.zero;
        }

        private bool FindRoom(int index)
        {
            Queue<int> openList = new Queue<int>();
            Queue<int> closedList = new Queue<int>();
            openList.Enqueue(index); //queue index to the open list
            int c = 0; // counter
            if (index > -1 && index < Mathf.FloorToInt(this.m_size.x * this.m_size.y)) //check if we're inside the area
            {
                if (m_maze[index].GetType() == typeof(WallCell))
                {
                    //UnityEngine.Debug.Log("WallCell at: " + index.ToString());
                    return false;
                }
                while (openList.Count != 0)
                {
                    int ind = openList.Dequeue(); //take ind out of open list
                    if (!IsInQueue(closedList, ind)) //if not in closed list
                    {
                        if (IsRoomAlreadyFound(ind)) //but already found
                        {
                            return false; //skip
                        }
                        closedList.Enqueue(ind); //add to closed list
                        this.m_indices[this.m_indicesCount] = ind; //indices = indexed rooms, saved for easy checking
                        this.m_indicesCount++; //rooms amount
                    }
                    int x = Mathf.FloorToInt(ind % this.m_size.x); //m_size is a vector so all the casting
                    int y = Mathf.FloorToInt(ind / this.m_size.y);
                    Vector2 xy = new Vector2(x, y);

                    //this for loop adds adjacent rooms to open list to be checked if they're already found
                    for (int i = 0; i < m_directions.Length; ++i) //check the cell in each direction
                    {
                        Vector2 txy = xy + m_directions[i];
                        int newInd = Mathf.FloorToInt(txy.y * this.m_size.x + txy.x);
                        if (newInd > -1 && newInd < Mathf.FloorToInt(this.m_size.x * this.m_size.y)) //if inside
                        {

                            if (m_maze[newInd].GetType() != typeof(WallCell)) //If not a wall cell
                            {

                                if (!IsInQueue(closedList, newInd)) //not in closed list
                                {

                                    if (!IsInQueue(openList, newInd)) //not in openlist --> add to openlist
                                    {
                                        openList.Enqueue(newInd);
                                    }
                                }
                            }
                        }
                    }

                    if (c > 100000)
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
            m_indicesCount = 0;
            Cell c = this.m_maze[index];

            if (c.GetType() != typeof(WallCell)) //Check if the index we're at is a room already
            {
                return;
            }
            int w = (int)this.m_size.x; //(int) <-- casting to int
            int indLeft = index - 1;
            int indRight = index + 1;
            int indUp = index - w;
            int indDown = index + w;
            bool lst1 = FindRoom(indLeft);
            bool lst2 = FindRoom(indRight);
            bool lst3 = FindRoom(indUp);
            bool lst4 = FindRoom(indDown);
            int count = (lst1 ? 1 : 0) + (lst2 ? 1 : 0) + (lst3 ? 1 : 0) + (lst4 ? 1 : 0); //if true return one for each
            if (count == 2) //is it even possible to have more than two
            {
                this.m_maze[index] = c.ToRoomCell(); //override with new type of room cell
            }

        }


        private void IterateMaze()
        {
            Random.InitState(m_seed);
            for (int i = 0; i < m_iterations; ++i)
            {
                int index = Random.Range(0, m_maze.Length);
                FindRooms(index);
            }
        }

        private void InitMaze(int w, int h)
        {
            int c = w * h;
            m_indices = new int[c]; //room indices
            this.m_size = new Vector2(w, h);
            m_maze = new Cell[c]; //maze is a list of cells matching room indices
            int i = 0;
            for (int y = 0; y < h; y++) //for height
            {
                for (int x = 0; x < w; x++) //for width
                {
                    Cell cell = null;
                    int yoe = y % 2;
                    int xoe = x % 2;
                    if (yoe == 1 && xoe == 1) //TODO: walls on each end. Currently every second in row and column
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

}
