using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Cell cell;
    public int cellCount;
    public Cell[,] cells=new Cell[9,15];
    public int openCell = 0;
    public Text text;
    
    public bool gameOver;
    public bool win;
    public int mineCount = 8;

    public InputField input;
    public GameObject panel;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }



    private void Update()
    {
        if (win || gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
         
         

    }



    public void RandomMine()
    {
        for(int i = 0; i < mineCount; i++)
        {
            int rows = Random.Range(0, 9);
            int colums = Random.Range(0, 15);
            
            if (cells[rows, colums].mine)
            {
                i--;
                
            }
            else
            {
                 
                cells[rows, colums].mine = true;
            }
             

        }
    }
     
    public void CheckCell(int colums,int rows)
    {
        if (!win&&!gameOver)
        {
            if (cells[rows, colums].mine)
            {
                text.text = "You Lose\nPress R To Restart";
                gameOver = true;
                text.gameObject.SetActive(true);
                AllOpen();
                return;
            }
            //Cell[] cellLst = new Cell[9];
            List<Cell> cellLst = new List<Cell>();
            int mineCount = 0;
            
            for (int i = rows - 1; i < (rows - 1) + 3; i++)
            {
                for (int j = colums - 1; j < (colums - 1) + 3; j++)
                {
                   
                    if (i < 0 || j < 0 || i > 8 || j > 14)
                    {

                        continue;
                    }
                    if (cells[i, j].open)
                    {
                        continue;
                    }
                    cellLst.Add(cells[i, j]);
                    if (cells[i, j].mine)
                    {

                        mineCount++;
                    }
                    
                }
            }
            if (mineCount != 0)
            {
                cells[rows, colums].Open();
                openCell++;
                cells[rows, colums].mineCountText.text = mineCount.ToString();
            }
            else if(mineCount==0)
            {
                
                for(int i = 0; i < cellLst.Count; i++)
                {
                    openCell++;
                    cellLst[i].Open();
                }
                 
            }
            if (openCell == this.cellCount - this.mineCount)
            {
                text.text = "You Win!!!\nPress R To Restart";
                win = true;
                text.gameObject.SetActive(true);
                AllOpen();
                return;
            }
        }
         
    }
    
    void AllOpen()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                
                cells[i, j].Open();


            }
        }
    }

    public void Submit()
    {
        if (input.text != ""&&input.text!="0")
        {
            mineCount = int.Parse(input.text);
            panel.SetActive(false);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Cell cell = Instantiate(this.cell, GameObject.Find("Cells").transform);
                    cell.rows = i;
                    cell.colums = j;
                    cells[i, j] = cell;




                }
            }
            RandomMine();
        }
         
    }
}
