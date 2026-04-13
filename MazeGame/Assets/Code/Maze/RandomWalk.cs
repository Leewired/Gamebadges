using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MazeGame.Maze
{
    public class RandomWalk : MonoBehaviour
    {
        private List<bool> m_cells = new List<bool>();
        public List<int> m_walkers = new List<int>();
        public int m_gridSize = 50;
        public int m_seed = 42;
        public int m_iterations = 1000;
        private int[] m_directions = null;
        public float m_cellSize = 1.0f;

        private void Start()
        {
            m_directions = new int[4]; //saving directions for easy using
            m_directions[0] = -1;
            m_directions[1] = -m_gridSize;
            m_directions[2] = 1;
            m_directions[3] = m_gridSize;

            for (int i = 0; i < m_gridSize * m_gridSize; i++)
            {
                m_cells.Add(true); // True = a wall.
            }

            StartCoroutine(WaitBeginning());
        }

        private IEnumerator WaitBeginning()
        {
            yield return new WaitForSeconds(2f); // wait so we can switch to scene view
            StartCoroutine(Iterate());
            yield return null;
        }

        private IEnumerator Iterate()
        {
            Random.InitState(m_seed); //seed for replicatable results

            for (int j = 0; j < m_iterations; ++j)
            {
                for (int i = 0; i < m_walkers.Count; i++)
                {
                    m_cells[m_walkers[i]] = false; //False = a floor

                    int v = Random.Range(0, 4);
                    int new_Ind = m_walkers[i] + m_directions[v];

                    if (IsInside(new_Ind))
                    {
                        m_walkers[i] = new_Ind;
                    }
                }
                yield return new WaitForSeconds(.1f);
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

        private void OnDrawGizmos() //debugging
        {
            for (int i = 0; i < m_cells.Count; ++i)
            {
                float x = (i % this.m_gridSize);
                float y = (i / this.m_gridSize);
                Vector3 p = new Vector3(x, 0f, y);
                if (m_cells[i])
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                for (int j = 0; j < m_walkers.Count; j++)
                {
                    if (i == m_walkers[j])
                    {
                        Gizmos.color = Color.yellow;
                    }
                }

                Gizmos.DrawCube(p, new Vector3(m_cellSize, 0.1f, m_cellSize));
            }
        }
    }

}