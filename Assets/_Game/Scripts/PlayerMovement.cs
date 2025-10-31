using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir;
    private Animator animator;
    private Coroutine speedBoostCoroutine;

    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 3f;

    private bool isPaused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        GamePauseManager.OnPaused += HandlePause;
        GamePauseManager.OnResumed += HandleResume;
    }

    void OnDisable()
    {
        GamePauseManager.OnPaused -= HandlePause;
        GamePauseManager.OnResumed -= HandleResume;
    }

    void Update()
    {
        if (isPaused) return;

        MovementHandler();
        AnimationHandler();
    }

    private void MovementHandler()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb.velocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > 0.01f)
            lastMoveDir = moveInput;
    }

    private void AnimationHandler()
    {
        float speed = rb.velocity.magnitude;
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("Speed", speed);
        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

    public void ApplySpeedBoost()
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoost(speedMultiplier, duration));
    }

    private IEnumerator SpeedBoost(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (!GamePauseManager.IsPaused)
                elapsed += Time.deltaTime;

            yield return null;
        }

        moveSpeed /= multiplier;
        speedBoostCoroutine = null;
    }


    private void HandlePause()
    {
        isPaused = true;
        rb.velocity = Vector2.zero;
        animator.speed = 0f;
    }

    private void HandleResume()
    {
        isPaused = false;
        animator.speed = 1f;
    }
}
