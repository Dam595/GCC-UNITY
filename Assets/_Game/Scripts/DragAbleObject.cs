using UnityEngine;

public class DragAbleObject : MonoBehaviour
{
    private bool isDraging = false;
    private Transform originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDraging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    public void IsDragingEnable()
    {
        isDraging = true;
    }
    public void IsDragingDisable()
    {
        isDraging = false;
    }
}
