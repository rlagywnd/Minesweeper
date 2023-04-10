using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
     
    public bool mine;
    public Text mineCountText;
    public int colums;
    public int rows;
    public bool open;
    public Sprite mineImage;
    bool onPointer;
    public void Click()
    {
         
        
        GameManager.instance.CheckCell(colums,rows);
    }
    public void Open()
    {
        if (mine)
        {
            GetComponent<Image>().sprite= mineImage;
            GetComponent<Image>().color=Color.white;
        }
        GetComponent<Image>().color = new Color(0.6698113f, 0.6698113f, 0.6698113f);
        GetComponent<Button>().interactable = false;
        open = true;
    }

    public void Update()
    {
        if (onPointer)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (GetComponent<Image>().color == Color.red)
                {
                    GetComponent<Image>().color = Color.white;
                }
                else
                {
                    GetComponent<Image>().color = Color.red;
                }

            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointer = true;
          
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointer = false;
    }
}
