using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MazeGame.Maze
{
    public class CellularAutomata : MonoBehaviour
    {
        private List<bool> m_cells = new List<bool>();
        private List<bool> m_prevCells = new List<bool>();
        public int m_gridSize = 50;
        public float m_cellSize = 1f;
        public int m_seed = 43;
        public int m_iterations = 100;

        private void Start()
        {
            UnityEngine.Random.InitState(m_seed);

            for (int i = 0; i < m_gridSize * m_gridSize; i++) // fill the cells
            {
                int v = UnityEngine.Random.Range(0, 2); // 0 or 1

                if (v == 0)
                {
                    m_cells.Add(false);
                }
                else
                {
                    m_cells.Add(true);
                }
            }
            StartCoroutine(WaitBeginning());
        }

        private IEnumerator WaitBeginning()
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine (Iterate());
            yield return null;
        }

        private IEnumerator Iterate()
        {
            for (int j = 0; j < m_iterations; ++j)
            {
                m_prevCells.Clear();

                for (int i = 0; i < m_gridSize * m_gridSize; ++i) //create cells
                {
                    m_prevCells.Add(m_cells[i]);
                }

                for (int y = 0; y < m_gridSize; y++) // figure this out
                {
                    for (int x = 0; x < m_gridSize; ++x)
                    {
                        int ind = y * m_gridSize + x;
                        int c = GetKernel(ind);

                        if (c < 4)
                        {
                            m_cells[ind] = true;
                        }
                        else
                        {
                            m_cells[ind] = false;
                        }
                    }
                }
                yield return new WaitForSeconds(.1f);
            }            
            yield return null;
        }

        private int GetKernel(int index)
        {
            int s = m_gridSize;
            int[] kernel =
            {
                0, -1, -s-1,
                -s, -s+1, 1,
                s+1, s, s-1
            }; // this is a 3x3 kernel

            int[] kvalues =
            {
                index + kernel[0], index + kernel[1], index + kernel[2],
                index + kernel[3], index + kernel[4], index + kernel[5],
                index + kernel[6], index + kernel[7], index + kernel[8]
            };

            int c = 0;
            int y = Mathf.FloorToInt((float)index / (float)m_gridSize); //dealing with grids, important
            int x = Mathf.FloorToInt((float)index % (float)m_gridSize);

            if (x < 1 || x > m_gridSize - 2 || y < 1 || y > m_gridSize - 2) //if out of bounds, or at the edge
            {
                return 5;
            }

            for (int i = 0; i < kvalues.Length; ++i)
            {
                c += Convert.ToInt32(m_prevCells[kvalues[i]]);
            }

            return c;
        }

        private void OnDrawGizmos() //debugging
        {
            for (int i = 0; i < m_cells.Count; ++i)
            {
                float x = (i % this.m_gridSize);
                float y = (i / this.m_gridSize);
                Vector3 p = new Vector3(x, 0f, y);

                if (m_cells[i]) //True cell = black
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawCube(p, new Vector3(m_cellSize, 0.1f, m_cellSize));
            }
        }
    }

}
