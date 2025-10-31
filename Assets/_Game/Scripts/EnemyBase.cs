using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float detectionRange = 5f;

    [Header("Components")]
    protected Rigidbody2D rb;
    protected Transform player;

    protected int currentHealth;
    protected bool isDead = false;
    protected bool isPaused = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentHealth = maxHealth;
    }

    protected virtual void OnEnable()
    {
        GamePauseManager.OnPaused += HandlePaused;
        GamePauseManager.OnResumed += HandleResumed;
    }

    protected virtual void OnDisable()
    {
        GamePauseManager.OnPaused -= HandlePaused;
        GamePauseManager.OnResumed -= HandleResumed;
    }

    protected virtual void Update()
    {
        if (isDead || isPaused) return;
        HandleMovement();
    }

    protected abstract void HandleMovement();

    public virtual void TakeDamage(int amount)
    {
        if (isDead || isPaused) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 0.5f);
    }
    protected virtual void HandlePaused()
    {
        isPaused = true;
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    protected virtual void HandleResumed()
    {
        isPaused = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
