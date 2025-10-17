using UnityEngine;

public class GridCell : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2Int gridPos;
    public DragAbleObject occupiedObject;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetGridPosition(int x, int y)
    {
        gridPos = new Vector2Int(x, y);
    }

    void OnMouseEnter()
    {
        sr.color = Color.yellow;
    }

    void OnMouseExit()
    {
        sr.color = Color.white;
    }
    public bool IsEmpty()
    {
        return occupiedObject == null;
    }

    public void SetOccupied(DragAbleObject obj)
    {
        occupiedObject = obj;
    }

    public void ClearOccupied()
    {
        occupiedObject = null;
    }

}
