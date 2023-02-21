using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	public void PlayButton()
	{
		SceneManager.LoadScene(1);
	}

	public void QuitButton()
	{
		Application.Quit();
	}

	// Other options can be selected like so

}
