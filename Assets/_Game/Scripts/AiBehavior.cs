using System.Collections;
using UnityEngine;

public class AiBehavior : MonoBehaviour
{
    [SerializeField] private GameObject chatBox;
    [SerializeField] private float activeDuration = 2f;

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            StartCoroutine(ShowChatBoxOnce());
        }
    }
    /// <summary>
    /// active ChatBox trong 2s sau đó tắt đi, có biến flag để đảm bảo chỉ active 1 lần
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowChatBoxOnce()
    {
        hasActivated = true;
        chatBox.SetActive(true);
        yield return new WaitForSeconds(activeDuration);
        chatBox.SetActive(false);
    }

   
}
