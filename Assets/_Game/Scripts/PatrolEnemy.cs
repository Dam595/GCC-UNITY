using UnityEngine;

public class PatrolEnemy : EnemyBase
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float arriveThreshold = 0.1f;
    private int currentPoint = 0;
    private Transform target;
     
    protected override void Awake()
    {
        base.Awake();
        if (patrolPoints.Length > 0)
            target = patrolPoints[0];
    }

    protected override void HandleMovement()
    {
        if (target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < arriveThreshold)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            target = patrolPoints[currentPoint];
        }
    }
}
