using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	PauseAction action;
	public static bool paused = false;
	public GameObject gameMenu;
	public GameObject formula;
	public GameObject controls;
	public GameObject soundManager;
	public GameObject FlipHelperPoint;
	public GameObject FlipPanel;
	public GameObject BGM;
	public TerrainCollider terrainColl;

	public Slider VolumeSlider;
	public TextMeshProUGUI VolumeNum;

	public TMP_Dropdown dropdown;
	public TextMeshProUGUI speedNumber;
	public TextMeshProUGUI GearNumber;

	private Ray flipRay;
	private bool showControls;
	private float flipTimer = 0;
	private bool hitTerrain;

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
		controls.SetActive(false);
		GearNumber.text = soundManager.GetComponent<SoundManager>().transmission.ToString();
		hitTerrain = false;
		FlipPanel.SetActive(false);

        VolumeSlider.onValueChanged.AddListener((value) => { VolumeNum.text = value.ToString("0.0"); });

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

	public void ShowControls()
	{
		if(showControls == true)
		{
			controls.SetActive(false);
			showControls = false;
		}
		else
		{
			controls.SetActive(true);
			showControls = true;
		}
			
	}

    void FixedUpdate()
	{

		BGM.GetComponent<AudioSource>().volume = float.Parse(VolumeNum.text);

		if (formula.GetComponent<CarController>().formulaSpeed > -0.1f && formula.GetComponent<CarController>().formulaSpeed < 0.1f)
		{
			speedNumber.text = (formula.GetComponent<CarController>().formulaSpeed).ToString("0.00");
			GearNumber.text = "N";
		}
		else if (formula.GetComponentInChildren<WheelCollider>().rpm >= 0) // A not so great way to determine if the car is going backwards, but it's probably the best way I could think of.
		{
			speedNumber.text = (formula.GetComponent<CarController>().formulaSpeed).ToString("0.00");
			GearNumber.text = soundManager.GetComponent<SoundManager>().transmission.ToString();
		}
		else
		{
			speedNumber.text = (formula.GetComponent<CarController>().formulaSpeed).ToString("-0.00");
			GearNumber.text = "R";
		}


		RaycastHit hit;
		flipRay = new Ray(formula.transform.position, FlipHelperPoint.transform.position - formula.transform.position);

		if (terrainColl.Raycast(flipRay, out hit, 25f))
		{
			hitTerrain = true;
		}

		if (!hitTerrain)
			flipTimer = 0;

		flipTimer += Time.deltaTime;
		hitTerrain = false;
		FlipPanel.SetActive(false);

		if (flipTimer > 3f && formula.GetComponent<CarController>().formulaSpeed < 4)
		{
			FlipPanel.SetActive(true);
		}

		Debug.DrawRay(formula.transform.position + new Vector3(0,0.8f,0), FlipHelperPoint.transform.position - formula.transform.position, Color.cyan);
	}
}