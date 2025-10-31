using System;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static event Action OnPaused;
    public static event Action OnResumed;
    public static bool IsPaused { get; private set; }

    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public static void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public static void SetPaused(bool paused)
    {
        if (IsPaused == paused) return;

        IsPaused = paused;
        if (paused)
        {
            Debug.Log("Game Paused");
            OnPaused?.Invoke();
        }
        else
        {
            Debug.Log("Game Resumed");
            OnResumed?.Invoke();
        }
    }
}
