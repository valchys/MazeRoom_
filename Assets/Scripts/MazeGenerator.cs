using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    [SerializeField]
    private MazeCell _mazeCellPrefab;
    [SerializeField]
    private int _mazeWidth;
    [SerializeField]
    private int _mazeDepth;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject finishPrefab;
    private MazeCell[,] _mazeGrid;
    

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth,_mazeDepth];
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GameObject finishPointObj = Instantiate(finishPrefab);
        finishPointObj.transform.position = new Vector3(0, 0, _mazeDepth - 1);


        GenerateMaze (null, _mazeGrid[0,0]);
    }

    private void GenerateMaze (MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        new WaitForSeconds(0.05f);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }

        } while (nextCell != null);

        PlaceRandomEnemies();
    }

    private void PlaceRandomEnemies()
    {
        int enemyCount = 5;

        for (int i = 0; i < enemyCount; i++)
        {
            MazeCell randomCell = GetRandomUnvisitedCell();

            if (randomCell!= null)
            {
                GameObject enemy = Instantiate(enemyPrefab, randomCell.transform.position, Quaternion.identity);
            }
        }
    }

    private MazeCell GetRandomUnvisitedCell()
    {
        List<MazeCell> unvisitedCells = new List<MazeCell>();

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                if (!_mazeGrid[x, z].IsVisited)
                {
                    unvisitedCells.Add(_mazeGrid[x, z]);
                }
            }
        }

        if (unvisitedCells.Count > 0)
        {
            return unvisitedCells[Random.Range(0, unvisitedCells.Count)];
        }

        return null;
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[ x + 1, z];
            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }
        if (x - 1 >= 0){
            var cellToLeft = _mazeGrid[x-1,z];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }
        if (z + 1 < _mazeDepth)
        {
            var cellUp = _mazeGrid[ x, z + 1];
            if (cellUp.IsVisited == false)
            {
                yield return cellUp;
            }
        }
        if (z - 1 >= 0){
            var cellDown = _mazeGrid[x, z - 1];
            if (cellDown.IsVisited == false)
            {
                yield return cellDown;
            }
        }
        
    }
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearTopWall();
            currentCell.ClearBottomWall();
            return;
        }
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBottomWall();
            currentCell.ClearTopWall();
            return;
        }


    }

    void Update()
    {
        
    }
}
