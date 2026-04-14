using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MazeGame.Maze
{
    public class RandomWalk : MonoBehaviour
    {
        //initialize fields for use
        private List<bool> m_cells = new List<bool>(); //list to hold each cell with Boolean values, True for wall cell, False for a floor cell for example
        public List<int> m_walkers = new List<int>(); //Each drunkard, list of Ints. Ints we can use to give the respectable drunkards a starting square or index
        public int m_gridSize = 50; //length of row
        public int m_seed = 42; //for replicatable results, and a less disappointed lecturer 
        public int m_iterations = 1000;
        private int[] m_directions = null; //list for ints to store directions for less laborius use
        public float m_cellSize = 1.0f;

        private void Start()
        {
            m_directions = new int[4]; //saving directions for easy using
            m_directions[0] = -1;
            m_directions[1] = -m_gridSize; //"up" a row (although we still work in 1D)
            m_directions[2] = 1;
            m_directions[3] = m_gridSize; //"down" a row

            for (int i = 0; i < m_gridSize * m_gridSize; i++)
            {
                m_cells.Add(true); // Create a grid's worth of wall cells.
            }

            StartCoroutine(WaitBeginning());
        }

        private IEnumerator WaitBeginning()
        {
            yield return new WaitForSeconds(2f); // wait so we can switch to scene view
            StartCoroutine(Iterate()); //let's iterate
            yield return null;
        }

        private IEnumerator Iterate()
        {
            Random.InitState(m_seed); //seed for replicatable results

            for (int j = 0; j < m_iterations; ++j) //for each iteration
            {
                for (int i = 0; i < m_walkers.Count; i++) //for each drunkard
                {
                    m_cells[m_walkers[i]] = false; //Cell that matches a drunkard = a floor

                    int v = Random.Range(0, 4); //random 0 to 3 to give for direction selection
                    int new_Ind = m_walkers[i] + m_directions[v]; //new index to 'place' a drunkard in

                    if (IsInside(new_Ind)) //if the new index is inside the grid. If false, stay still
                    {
                        m_walkers[i] = new_Ind; //if true, 'move' to new index
                    }
                }
                yield return new WaitForSeconds(.1f); //time to wait between iterations
            }            
            yield return null;
        }

        private bool IsInside(int v) //check if we are inside the area
        {
            if (v < 0)
            {
                return false;
            }
            if (v > m_gridSize * m_gridSize)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnDrawGizmos() //debugging and visualization
        {
            for (int i = 0; i < m_cells.Count; ++i) //for each cell. This grid work is new to me
            {
                float x = (i % this.m_gridSize); //'this.' doesn't seem necessary (seems to work without) was there a reason to add it?
                float y = (i / this.m_gridSize); // how do these form a grid from the list --> x = 0, 1, 2, 3 ... y = 0, 0, 0 until gridSize is reached, then x = 0, 1, 2 ... y = 1, 1, 1 => put them in a vector
                Vector3 p = new Vector3(x, 0f, y); // we don't care about Y-axis (but we need it, center for DrawCube needs three values for the axis) which would be the height, x=X y=Z

                if (m_cells[i]) //if the cell is True aka a wall
                {
                    Gizmos.color = Color.black; //a black wall
                }
                else
                {
                    Gizmos.color = Color.green; //a green herb - some life in the darkness
                }

                for (int j = 0; j < m_walkers.Count; j++) //for each drunkard
                {
                    if (i == m_walkers[j]) //if drunkard is here (the values match)
                    {
                        Gizmos.color = Color.yellow; //smells of piss, or maybe just strong cider
                    }
                }

                Gizmos.DrawCube(p, new Vector3(m_cellSize, 0.1f, m_cellSize)); //give center, p (defined above) and size, tall or maybe not this time.
            }
        }
    }

}