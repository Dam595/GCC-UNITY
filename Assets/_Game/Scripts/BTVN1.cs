using UnityEngine;

public class BTVN1 : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField] private Vector2 cellSize = new Vector2(1, 1);
    [SerializeField] private GameObject cellPrefab;

    public float cellGap = 0f;
    private GridCell[,] cells;
    private float offsetX, offsetY;
    private LayerMask dragLayerMask;
    private DragAbleObject currentDraggingObject;
    private Vector3 originalPos;
    private GridCell originalCell;
    /// <summary>
    /// Note lại những lỗi cần sửa: Phải đặt tên method cho dễ hiểu, tạo method dễ tái sử dụng
    /// </summary>
    void Start()
    {
        InitGridOffset();
        InitGridCells();
        dragLayerMask = LayerMask.GetMask("DragLayer");
        
    }

    private void InitGridOffset()
    {
        offsetX = -gridSize.x * cellSize.x / 2f + cellSize.x / 2f;
        offsetY = -gridSize.y * cellSize.y / 2f + cellSize.y / 2f;
    }

    private void InitGridCells()
    {
        cells = new GridCell[gridSize.x, gridSize.y];
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 2f, dragLayerMask);
            if(hit.collider != null)
            {
                currentDraggingObject = hit.collider.GetComponent<DragAbleObject>();
                Vector2Int cellPos = GetCellFromWorldPos(currentDraggingObject.transform.position);
                if (cellPos.x >= 0)
                {
                    originalCell = cells[cellPos.x, cellPos.y];
                }
                originalPos = currentDraggingObject.transform.position;
                if(originalCell != null)
                {
                    originalCell.ClearOccupied();
                }
            }
            OnMouseDown();
        }
        if (Input.GetMouseButton(0))
        {
            if (currentDraggingObject != null)
            {
                currentDraggingObject.IsDragingEnable();
                
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (currentDraggingObject != null)
            {
                currentDraggingObject.IsDragingDisable();
                Vector3 dropPos = currentDraggingObject.transform.position;
                Vector2Int cellPos = GetCellFromWorldPos(dropPos);
                

                if (cellPos.x >= 0)
                {
                    GridCell cell = cells[cellPos.x, cellPos.y];
                    if (cell.IsEmpty())
                    {
                        Vector3 cellWorldPos = GetWorldPositionOfCell(cellPos.x, cellPos.y);
                        currentDraggingObject.transform.position = cellWorldPos;
                        cell.SetOccupied(currentDraggingObject);
                    }
                    else
                    {
                        ReturnToOriginalPosition();
                    }
                }
                else
                {
                    ReturnToOriginalPosition();
                }
                currentDraggingObject = null;
            }

        }

    }
    private void ReturnToOriginalPosition()
    {
       if(originalCell != null)
        {
            originalCell.SetOccupied(currentDraggingObject);
        }
       currentDraggingObject.transform.position = originalPos;
    }
    private void OnDrawGizmos()
    {
        InitGridOffset();
        
        for (int i=0; i<gridSize.x; i++)
        {
            for(int j=0; j<gridSize.y; j++)
            {
                Vector3 position = new Vector3(i * cellSize.x + offsetX, j * cellSize.y + offsetY, 0);
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

        Vector2Int cellPos = GetCellFromWorldPos(mouseWorldPos);
        if (cellPos.x >= 0)
        {
            Debug.Log($"Clicked on cell: ({cellPos.x}, {cellPos.y})");
        }
    }


    private void Debugging(string message, GameObject gameObject)
    {
        Debug.Log(message + " from " + gameObject);
    }
}
