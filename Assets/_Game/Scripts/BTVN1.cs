using UnityEngine;

public class BTVN1 : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField] private Vector2 cellSize = new Vector2(1, 1);
    [SerializeField] private GameObject cellPrefab;
    public float cellGap = 0f;

    private GridCell[,] cells;
    private float offsetX, offsetY;

    void Start()
    {
        cells = new GridCell[gridSize.x, gridSize.y];

        offsetX = -gridSize.x * cellSize.x / 2f + cellSize.x / 2f;
        offsetY = -gridSize.y * cellSize.y / 2f + cellSize.y / 2f;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 pos = new Vector3(
                    x * (cellSize.x + cellGap) + offsetX,
                    y * (cellSize.y + cellGap) + offsetY,
                    0
                );

                GameObject cellObj = Instantiate(cellPrefab, transform.position + pos, Quaternion.identity, transform);
                cells[x, y] = cellObj.GetComponent<GridCell>();
                cells[x, y].SetGridPosition(x, y);
            }
        }
    }

    public Vector2Int GetCellFromWorldPos(Vector3 worldPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        int x = Mathf.FloorToInt((localPos.x - offsetX + cellSize.x / 2) / cellSize.x);
        int y = Mathf.FloorToInt((localPos.y - offsetY + cellSize.y / 2) / cellSize.y);

        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            return new Vector2Int(x, y);

        return new Vector2Int(-1, -1);
    }

    public Vector3 GetWorldPositionOfCell(int x, int y)
    {
        return transform.position + new Vector3(
            x * cellSize.x + offsetX,
            y * cellSize.y + offsetY,
            0
        );
    }

    public Vector2Int GridSize => gridSize;

    /// <summary>
    /// Từ phần dưới này mới là btvn, phần trên và những code khác iem làm thêm để cho đẹp thoy =)), tại cái này trc em làm trong con Witch Blaster r
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnMouseDown();
    }
    private void OnDrawGizmos()
    {
        float offSetX = (float)(-gridSize.x * cellSize.x / 2) + cellSize.x / 2;
        float offSetY = (float)(-gridSize.y * cellSize.y / 2) + cellSize.y / 2;
        
        for (int i=0; i<gridSize.x; i++)
        {
            for(int j=0; j<gridSize.y; j++)
            {
                Vector3 position = new Vector3(i * cellSize.x + offSetX, j * cellSize.y + offSetY, 0);
                Vector3 size = new Vector3(cellSize.x, cellSize.y, 0);
                Gizmos.color = Random.ColorHSV();
                Gizmos.DrawWireCube(position, size);
            }
        }
    }
    private void OnMouseDown()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float offSetX = -gridSize.x * cellSize.x / 2f;
        float offSetY = -gridSize.y * cellSize.y / 2f;

        float localX = mouseWorldPos.x - offSetX;
        float localY = mouseWorldPos.y - offSetY;

        int col = Mathf.FloorToInt(localX / cellSize.x);
        int row = Mathf.FloorToInt(localY / cellSize.y);

        if (col >= 0 && col < gridSize.x && row >= 0 && row < gridSize.y)
        {
            Debug.Log($"Clicked on cell: (Row = {row}, Col = {col})");
        }
        else
        {
            Debug.Log("Clicked outside");
        }
    }
}
