using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Animator anime;
    [SerializeField] private SpriteRenderer sr; 
    [SerializeField] private Sprite number;
    [SerializeField] private SpriteRenderer flagIcon;
    private bool flag = false;
    public bool isBomb = false;
    private int surroundingMines;
    private bool isClick = false;
    public bool IsOpen { get; private set; }
    private void OnMouseDown()
    {
        if (flag)
        {
            return;
        }
        if (!isClick)
        {
            isClick = true;
            GameManager.Instance.OpenCell(this); 
        }
    }

    //애니메이션 재생후 자동으로 호출됨
    public void OpenPanel()
    {
        if (isBomb)
        {
            Boom();
        }
        sr.sprite = number;
        
    }

    public void OpenCell(Sprite sprite)
    {
        anime.SetTrigger("Open");
        GameManager.Instance.openCellCount++;
        number = sprite;
        IsOpen = true; 
    }
    
    public void SetFlag(Sprite flag)
    {
        if(flagIcon.sprite == flag)
        {
            this.flag = false;
            flagIcon.sprite = null;
            return;
        }
        this.flag = true;
        flagIcon.sprite = flag;
    }
    public void Boom()
    {
        sr.color = Color.red; 
    }
}

