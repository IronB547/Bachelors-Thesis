using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    PauseAction action;
    public static bool paused = false;
    public GameObject gameMenu;
    public GameObject formula;
    public TMP_Dropdown dropdown;

    // Handles pausing and 
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

        // The dropdown list color would require way more time and there are way more important matters than this
        // maybe I will fix the color scheme later
        dropdown.SetValueWithoutNotify(2);
        dropdown.onValueChanged.AddListener(delegate { SelectedItem(dropdown); });
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

    // Dropdown selection

    void SelectedItem(TMP_Dropdown dropdown)
    {

        switch(dropdown.value){
            case 0:
                formula.GetComponent<CarController>().carDriveType = CarController.CarDriveType.FrontWheelDrive;
                break;
            case 1:
                formula.GetComponent<CarController>().carDriveType = CarController.CarDriveType.RearWheelDrive;
                break;

            case 2:
                formula.GetComponent<CarController>().carDriveType = CarController.CarDriveType.FourWheelDrive;
                break;

            default:
                break;
        }
        
    }

}
