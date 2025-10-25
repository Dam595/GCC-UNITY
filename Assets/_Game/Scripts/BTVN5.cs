using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTVN5 : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private Vector2 shrinkScale = new Vector2(0.7f, 0.7f);
    [SerializeField] private Vector2 expandScale = new Vector2(1.2f, 1.2f);
    [SerializeField] private float shrinkTime = 0.1f;
    [SerializeField] private float expandTime = 0.15f;
    [SerializeField] private float returnTime = 0.1f;

    [Header("Bounce Settings")]
    public float bounceHeight = 0.5f;
    [SerializeField] private float bounceUpTime = 0.15f;
    [SerializeField] private float bounceDownTime = 0.2f;
    [SerializeField] private Vector2 stretchUpScale = new Vector2(0.9f, 1.3f);
    [SerializeField] private Vector2 squashDownScale = new Vector2(1.3f, 0.8f);

    private Vector3 originalScale;
    private Vector3 originalPos;
    private Coroutine anim;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPos = transform.localPosition;
    }

    private void Start()
    {
        if (anim != null)
        {
            StopCoroutine(anim);
        }
        anim = StartCoroutine(AnimationSequence());
    }

    IEnumerator AnimationSequence()
    {
        // Thu nhỏ
        yield return ScaleToTargetScale(shrinkScale, shrinkTime);

        // Giãn to
        yield return ScaleToTargetScale(expandScale, expandTime);

        // Quay về scale ban đầu
        yield return ScaleToTargetScale(originalScale, returnTime);

        // Nhún nhảy
        yield return BounceRoutine();
    }

    IEnumerator ScaleToTargetScale(Vector2 targetScale, float duration)
    {
        Vector2 startScale = transform.localScale;
        float time = 0f;
        // đếm thời gian
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector2 newScale = Vector2.Lerp(startScale, targetScale, t);
            transform.localScale = new Vector3(newScale.x, newScale.y, 1f);
            yield return null;
        }

        transform.localScale = new Vector3(targetScale.x, targetScale.y, 1f);
    }

    IEnumerator BounceRoutine()
    {
        Vector3 startPos = transform.localPosition;
        // nhảy lên nè
        yield return MoveAndScale(startPos, startPos + Vector3.up * bounceHeight, originalScale, stretchUpScale, bounceUpTime);

        // rơi xuống nè
        yield return MoveAndScale(startPos + Vector3.up * bounceHeight, startPos, stretchUpScale, squashDownScale,bounceDownTime);

        // quay lại scale ban đầu nè
        yield return ScaleToTargetScale(originalScale, 0.5f);
    }

    IEnumerator MoveAndScale(Vector3 fromPos, Vector3 toPos, Vector2 fromScale, Vector2 toScale, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

            Vector2 newScale = Vector2.Lerp(fromScale, toScale, t);
            transform.localScale = new Vector3(newScale.x, newScale.y, 1f);

            yield return null;
        }

        transform.localPosition = toPos;
        transform.localScale = new Vector3(toScale.x, toScale.y, 1f);
    }
}
