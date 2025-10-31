using UnityEngine;

public class ChaseEnemy : EnemyBase
{
    protected override void HandleMovement()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
