using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CellFlagManager : MonoBehaviour
{ 
    LineRenderer lr;
    Vector3 mouseWorldPos;
    public Material lineMaterial;
    public GameObject flag;

    public SpriteRenderer bombFlag;
    public SpriteRenderer warningFlag;
    Sprite flagSprite;
    Cell selectCell;

    private void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (lr != null)
        {
            SelectFlag();
            mouseWorldPos.z = -5;
            lr.SetPosition(1, mouseWorldPos);
        }
        if (Input.GetMouseButtonDown(1))
        { 
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                selectCell = hit.collider.GetComponent<Cell>();
                if (selectCell != null)
                {
                    Vector3 cellPos = selectCell.transform.position;
                    cellPos.z = 0;
                    flag.transform.position = cellPos;
                    flag.SetActive(true);
                    AddLineRenderer(selectCell);
                    
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && lr != null)
        {
            selectCell.SetFlag(flagSprite);
            flag.SetActive(false);
            selectCell = null;
            Destroy(lr);
        }
    }

    void SelectFlag()
    {
        float posX = mouseWorldPos.x;
        if (posX < flag.transform.position.x)
        {
            warningFlag.color = Color.white;
            bombFlag.color = Color.red;
            flagSprite = bombFlag.sprite; 
        }
        else if (posX > flag.transform.position.x)
        {
            warningFlag.color = Color.red;
            bombFlag.color = Color.white;
            flagSprite = warningFlag.sprite; 
        }
        else
        {
            warningFlag.color = Color.white;
            bombFlag.color = Color.white;
            flagSprite = null;
            return;
        }
    }

    void AddLineRenderer(Cell cell)
    {
        lr = cell.AddComponent<LineRenderer>();
        lr.sortingOrder = 51;
        lr.positionCount = 2;
        lr.startWidth = 0.1f; // Ω√¿€¡° ±Ω±‚
        lr.endWidth = 0.1f;   // ≥°¡° ±Ω±‚
        lr.material = lineMaterial;
        Vector3 pos = cell.transform.position;
        pos.z = -5;
        lr.SetPosition(0, pos);
    }
}
