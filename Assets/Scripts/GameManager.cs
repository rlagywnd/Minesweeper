using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }    
    [SerializeField] private Array2D<Cell>[] cells;
    [SerializeField] private Transform firstCellPos;
    //[SerializeField] private Transform lastCellPos;
    //
    [SerializeField] private Sprite[] numbers;//셀 근처 8칸에 있는 폭탄 개수가 적혀있는 이미지
    [SerializeField] private int bombCount;//게임 내에 있는 폭탄 개수
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Text gameEndText;//game over 여부에따라 텍스트가 달라짐
    [SerializeField] private FadeManager fadeObject;
    private List<Vector2Int> bombLocations = new List<Vector2Int>();
    private WaitForSeconds autoFindDelays;
    private WaitForSeconds timer;
    [HideInInspector] public int openCellCount;//셀이 열릴때마다 1씩 증가
    public bool IsGameEnd { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        autoFindDelays = new WaitForSeconds(3f/(float)bombCount);
        timer = new WaitForSeconds(1);

    }
    
    void Start()
    {
        fadeObject.PlayAnime("FadeIn");
        Init();
        StartCoroutine(Timer()); 
    }
     

    public void OpenCell(Cell cell)
    {
        if (IsGameEnd)
        {
            return;
        }
        if (cell.isBomb)
        {
            cell.OpenCell(numbers[numbers.Length-1]);
            IsGameEnd = true;
            GameEnd(true);
            return;
        }

        int bombCount = GetCountBombsAroundCell(cell);
        if(bombCount > 0)
        { 
            cell.OpenCell(numbers[bombCount]);
        }
        else if (bombCount == 0)
        { 
            OpenAdjacentCells(cell, bombCount);
        } 
        if(openCellCount == cells.Length * cells[0].Length - this.bombCount)
        {
            IsGameEnd = true;
            GameEnd(false);
        }
    }

    void OpenAdjacentCells(Cell startCell,int bombCount)
    {
        if (startCell.IsOpen)
        {
            return;
        }
        if (startCell.isBomb)
        {
            return;
        }

        startCell.OpenCell(numbers[bombCount]); 
        if (bombCount > 0)
        {
            return;
        }

        Vector2Int[] dir = 
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),

            new Vector2Int(1,1),
            new Vector2Int(-1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1),
        };

        Vector2Int pos = IndexCalculation(startCell.transform);
        for(int i = 0;i < dir.Length; i++)
        {
            Vector2Int neighborCellsPos = pos + dir[i];
            if (IsValidIndex(neighborCellsPos))
            {
                Cell neighborCells = cells[neighborCellsPos.y][neighborCellsPos.x];
                int count = GetCountBombsAroundCell(neighborCells);
                OpenAdjacentCells(neighborCells, count);
            }
        }
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

    public void GameEnd(bool gameOver)
    {
        StartCoroutine(BombsAutoFind(gameOver));
        
    }
    public void RetryButton()
    {
        fadeObject.PlayAnime("FadeOut");
    }
    IEnumerator BombsAutoFind(bool gameOver)
    {
        for(int i = 0;i < bombLocations.Count; i++)
        {
            yield return autoFindDelays;
            Vector2Int bombPos = bombLocations[i];
            Cell bomb = cells[bombPos.y][bombPos.x];
            if (bomb.IsOpen)
            {
                continue;
            }
            bomb.OpenCell(numbers[numbers.Length-1]);
        }
        yield return autoFindDelays;
        if (gameOver)
        {
            gameEndText.text = "Game Over..";
            gameEndPanel.SetActive(true);
        }
        else
        {
            gameEndText.text = "You Win!!";
            gameEndPanel.SetActive(true);
        }
    }

    IEnumerator Timer()
    {
        int time = 0;
        while (!IsGameEnd)
        {
            yield return timer;
            time++;
            timerText.text = $"{(int)(time / 60)} : {time % 60}";
        }
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
        Vector2 pos = obj.position - firstCellPos.position;
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
        // 지뢰를 랜덤하게 배치
        for (int i = 0; i < bombCount; i++)
        {
            int row, col;
             
            do
            {
                row = Random.Range(0, cells[0].Length);
                col = Random.Range(0,cells.Length);
            }
            while (cells[col][row].isBomb);
            bombLocations.Add(new Vector2Int(row, col));
            cells[col][row].isBomb = true;
        }
    }
}
