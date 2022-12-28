using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    PauseAction action;
    public static bool paused = false;
    public GameObject gameMenu;

    private void Awake()
    {
        action = new PauseAction();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        gameMenu.SetActive(false);
        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void DeterminePause()
    {
        if (paused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameMenu.SetActive(true);
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        gameMenu.SetActive(false);
        paused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        gameMenu.SetActive(false);
        paused = false;
        SceneManager.LoadScene(0);
    }

}
