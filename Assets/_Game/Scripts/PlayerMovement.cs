using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir;
    private Animator animator;


    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float shockwaveCastRange = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovementHandler();
        AnimationHandler();
        if (Input.GetKeyDown(KeyCode.P))
        {
            DoPushWave();
        }
    }

    /// <summary>
    /// Code quản lý di chuyển của nhân vật người chơi
    /// </summary>
    private void MovementHandler()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb.velocity = moveInput * moveSpeed;
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveInput;
        }
    }

    /// <summary>
    /// Code quản lý Animation của nhân vật người chơi
    /// </summary>
    private void AnimationHandler()
    {
        float speed = rb.velocity.magnitude;
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("Speed", speed);
        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

    /// <summary>
    /// Push wave đẩy lùi các đối tượng xung quanh trong phạm vi shockwaveCastRange
    /// </summary>
    private void DoPushWave()
    {
        int layerMask = LayerMask.GetMask("NPC");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveCastRange, layerMask);

        foreach (var hit in hits)
        {
            ApplyKnockback(hit.transform);
        }
    }


    /// <summary>
    /// Applies a knockback force to the specified target.
    /// </summary>
    private void ApplyKnockback(Transform target)
    {
        Rigidbody2D rb2d = target.GetComponent<Rigidbody2D>();
        if (rb2d != null && rb2d != this.rb)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            rb2d.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Random.ColorHSV();
        Gizmos.DrawWireSphere(transform.position, shockwaveCastRange);
    }

}
