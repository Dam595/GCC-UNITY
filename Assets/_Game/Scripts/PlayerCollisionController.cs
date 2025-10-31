using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    private void OnTriggerEnter2D(Collider2D other)
    {
        IItem item = other.GetComponent<IItem>();
        if (item != null)
        {
            item.OnPickup(this);
        }
    }

    public void OnSpeedItemCollected()
    {
        if (movement != null)
        {
            movement.ApplySpeedBoost();
        }
    }
}
