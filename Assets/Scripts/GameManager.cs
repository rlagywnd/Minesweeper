using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }    
    [SerializeField] private Array2D<Cell>[] cells;
    [SerializeField] private Transform firstPos;
    [SerializeField] private Transform lastPos;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private int bombCount;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    { 
        Init();
    }
     
    
    public int GetCountBombsAroundCell(Cell cell)
    {
        int bombCount = 0;
        Transform cellPos = cell.transform;
        Vector2Int index = IndexCalculation(cellPos);
        for (int i = index.y - 1; i <= index.y + 1; i++)
        {
            for (int j = index.x - 1; j <= index.x + 1; j++)
            {
                Vector2Int surroundingIndex = new Vector2Int(j, i);
                if (!IsValidIndex(surroundingIndex) || index == surroundingIndex)
                {
                    continue;
                }

                if (cells[surroundingIndex.y][surroundingIndex.x].isBomb)
                {
                    bombCount++;
                }
            }
        } 

        return bombCount;
    }
    public Sprite GetCellCountSprite(Cell cell)
    {
        if (cell.isBomb)
        {
            return numbers[numbers.Length - 1];
        }
        int count = GetCountBombsAroundCell(cell);
          
        return numbers[count];
    }
    public Vector2Int IndexCalculation(Transform obj)
    {
        Vector2 pos = obj.position - firstPos.position;
        pos /= 0.55f; 
        int x = (int)Mathf.Round(pos.x);
        int y = (int)Mathf.Round(pos.y);
        Vector2Int result = new Vector2Int(x, y);
        return result;  
    }

    bool IsValidIndex(Vector2Int index)
    {
        if ((index.x >= 0 && index.x < cells[0].Length)
            &&
            (index.y >= 0 && index.y < cells.Length))
        {
            return true;
        }
        return false;
    }

    private void Init()
    { 
        // Áö·Ú¸¦ ·£´ýÇÏ°Ô ¹èÄ¡
        for (int i = 0; i < bombCount; i++)
        {
            int row, col;
             
            do
            {
                row = Random.Range(0,cells.Length);
                col = Random.Range(0,cells[0].array.Length);
            }
            while (cells[row][col].isBomb);
            print($"{row} , {col}");
            cells[row][col].isBomb = true;
        }
    }
}
