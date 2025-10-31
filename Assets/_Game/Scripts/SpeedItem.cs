using UnityEngine;

public class SpeedItem : MonoBehaviour, IItem
{
    public void OnPickup(PlayerCollisionController collisionController)
    {
        collisionController.OnSpeedItemCollected();
        Destroy(gameObject);
    }
}
