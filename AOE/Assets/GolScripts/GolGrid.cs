using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolGrid : MonoBehaviour
{
    private bool[,] cells;
    private Transform[,] cellObjects;

    public GameObject cellObjectPrefab;

    public void Initialize(int width, int height)
    {
        cells = new bool[width, height];
        cellObjects = new Transform[width, height];

        for (int x = 0; x <= cells.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= cells.GetUpperBound(1); y++)
            {
                GameObject newTile = PlaceTile(new Vector2(x, y));
                cellObjects[x, y] = newTile.transform;
            }
        }
    }

    public void UpdateState()
    {
        List<Vector2Int> birthCells = new List<Vector2Int>();
        List<Vector2Int> killCells = new List<Vector2Int>();

        for (int x = 0; x <= cells.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= cells.GetUpperBound(1); y++)
            {
                int neighborCount = CountNeighbors(new Vector2Int(x, y));
                if (cells[x, y] && neighborCount != 2 && neighborCount != 3)
                {
                    killCells.Add(new Vector2Int(x, y));
                }
                else if (!cells[x, y] && neighborCount == 3)
                {
                    birthCells.Add(new Vector2Int(x, y));
                }
            }
        }

        UpdateCells(birthCells, killCells);
    }

    private void UpdateCells(List<Vector2Int> births, List<Vector2Int> deaths)
    {
        foreach (Vector2Int birthCoord in births)
        {
            cells[birthCoord.x, birthCoord.y] = true;
            cellObjects[birthCoord.x, birthCoord.y].GetComponent<MeshRenderer>().material.color = Color.black;
        }

        foreach (Vector2Int deathCoord in deaths)
        {
            cells[deathCoord.x, deathCoord.y] = false;
            cellObjects[deathCoord.x, deathCoord.y].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private int CountNeighbors(Vector2Int cellCoord)
    {
        int neighbors = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int checkedCoord = new Vector2Int(cellCoord.x + x, cellCoord.y + y);
                if (checkedCoord != cellCoord && CheckValidCell(checkedCoord))
                {
                    if (cells[checkedCoord.x, checkedCoord.y])
                    {
                        neighbors++;
                    }
                }
            }
        }

        return neighbors;
    }

    private GameObject PlaceTile(Vector2 cellCoord)
    {
        return GameObject.Instantiate(cellObjectPrefab, cellCoord, Quaternion.identity, transform);
    }

    private bool CheckValidCell(Vector2 cellCoord)
    {
        return cellCoord.x > 0 && cellCoord.x <= cells.GetUpperBound(0) && cellCoord.y > 0 && cellCoord.y <= cells.GetUpperBound(1);
    }

    /*TEST*/private List<Vector2Int> InitialCells = new List<Vector2Int>()
            {
                new Vector2Int(2, 9),
                new Vector2Int(3, 8),
                new Vector2Int(1, 7),
                new Vector2Int(2, 7),
                new Vector2Int(3, 7),
                new Vector2Int(8, 1),
                new Vector2Int(8, 2),
                new Vector2Int(8, 3)
            };

    /*TEST*/private void Start()
    {
        Initialize(15, 15);
        UpdateCells(InitialCells, new List<Vector2Int>());
        StartCoroutine(UpdateTick());
    }

    /*TEST*/private IEnumerator UpdateTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            UpdateState();
        }
    }
}