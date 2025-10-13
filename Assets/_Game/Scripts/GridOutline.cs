using UnityEngine;

public class GridOutline : MonoBehaviour
{
    public BTVN1 grid;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2Int cell = grid.GetCellFromWorldPos(mouseWorldPos);

        if (cell.x >= 0)
        {
            transform.position = grid.GetWorldPositionOfCell(cell.x, cell.y);
            sr.enabled = true;
        }
        else
        {
            sr.enabled = false;
        }
    }
}
