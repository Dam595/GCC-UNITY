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
    [SerializeField] private float bounceHeight = 0.5f;
    [SerializeField] private float bounceUpTime = 0.15f;
    [SerializeField] private float bounceDownTime = 0.2f;
    [SerializeField] private Vector2 stretchUpScale = new Vector2(0.9f, 1.3f);
    [SerializeField] private Vector2 squashDownScale = new Vector2(1.3f, 0.8f);

    private Vector3 originalScale;
    private Vector3 originalPos;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPos = transform.localPosition;
    }

    private void Start()
    {
        // Co lại
        StartCoroutine(ScaleToTargetScale(shrinkScale, shrinkTime));
        Invoke(nameof(Expand), shrinkTime);
    }

    // Giãn ra
    private void Expand()
    {
        StartCoroutine(ScaleToTargetScale(expandScale, expandTime));
        Invoke(nameof(ReturnScale), expandTime);
    }

    // Về scale ban đầu
    private void ReturnScale()
    {
        StartCoroutine(ScaleToTargetScale(originalScale, returnTime));
        Invoke(nameof(StartBounceUp), returnTime);
    }
    
    IEnumerator ScaleToTargetScale(Vector2 targetScale, float duration)
    {
        Vector2 startScale = transform.localScale;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector2 newScale = Vector2.Lerp(startScale, targetScale, t);
            transform.localScale = new Vector3(newScale.x, newScale.y, 1f);
            yield return null;
        }
    }

    /// <summary>
    /// Code nhún nhảy
    /// </summary>
    private void StartBounceUp()
    {
        Vector3 startPos = originalPos;
        Vector3 topPos = originalPos + Vector3.up * bounceHeight;

        StartCoroutine(MoveAndScale(startPos, topPos, originalScale, stretchUpScale, bounceUpTime));
        Invoke(nameof(StartBounceDown), bounceUpTime);
    }

    private void StartBounceDown()
    {
        Vector3 startPos = originalPos;
        Vector3 topPos = originalPos + Vector3.up * bounceHeight;

        StartCoroutine(MoveAndScale(topPos, startPos, stretchUpScale, squashDownScale, bounceDownTime));
        Invoke(nameof(ReturnBounceScale), bounceDownTime);
    }

    private void ReturnBounceScale()
    {
        StartCoroutine(ScaleToTargetScale(originalScale, 0.3f));
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