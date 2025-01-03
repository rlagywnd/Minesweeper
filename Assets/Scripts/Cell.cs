using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite number;
    [SerializeField] private GameObject flagIcon;
    public bool isBomb = false;
    private int surroundingMines;
    private bool isClick = false;

    private void OnMouseDown()
    {
        if (!isClick)
        {
            isClick = true;
            CellOpen(); 
            isClick = false;
        }
    } 

    public void OpenPanel()
    {
        sr.sprite = number;
        if (isBomb)
        {
            Boom();
        }
    }
    public void CellOpen()
    {
        animator.SetTrigger("Open");
        number = GameManager.Instance.GetCellCountSprite(this);
    } 
    public void Boom()
    {
        sr.color = Color.red;
    }
}

