using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MedalManager : MonoBehaviour
{

	public TextMeshProUGUI BestTimeMinBox;
	public TextMeshProUGUI BestTimeSecBox;
	public TextMeshProUGUI BestTimeMilliBox;
	public TextMeshProUGUI BestTimeHundredthsBox;

	public Sprite Checkmark;
	public Sprite Cross;

    [SerializeField] private Image[] MedalImageChecks = new Image[4];

    public Image FinalMedal;
    
    [SerializeField] private Sprite[] MedalSprites = new Sprite[5];
	public TextMeshProUGUI MedalText;

    private float bronzeTime = 190;
    private float silverTime = 120;
	private float goldTime = 95;
    private float platinumTime = 85;


    void OnTriggerEnter()
	{

        float time = ConvertTime();

        if (GetComponent<LapTriggerLogic>().laps == 0)
		{
			DisplayMedal(time);
		}

		if(time < platinumTime)
		{
			MedalImageChecks[3].sprite = Checkmark;
			CheckPreviousTimes(3);
		}
		else if(time < goldTime)
		{
            MedalImageChecks[2].sprite = Checkmark;
            CheckPreviousTimes(2);
        }
		else if(time < silverTime)
		{
            MedalImageChecks[1].sprite = Checkmark;
			CheckPreviousTimes(1);
        }
		else if(time < bronzeTime)
		{
            MedalImageChecks[0].sprite = Checkmark;
        }

	}

    float ConvertTime()
	{
		return int.Parse(BestTimeMinBox.text.Substring(0, BestTimeMinBox.text.Length - 1)) * 60 + 
			int.Parse(BestTimeSecBox.text.Substring(0, BestTimeSecBox.text.Length - 1)) +
			float.Parse(BestTimeMilliBox.text) / 10 + float.Parse(BestTimeHundredthsBox.text) / 100;
	}


	void CheckPreviousTimes(int medals)
	{
		switch (medals)
		{
			case 1:
				MedalImageChecks[0].sprite = Checkmark;
				break;

			case 2:
				MedalImageChecks[0].sprite = Checkmark;
				MedalImageChecks[1].sprite = Checkmark;
				break;

			case 3:
				MedalImageChecks[0].sprite = Checkmark;
				MedalImageChecks[1].sprite = Checkmark;
				MedalImageChecks[2].sprite = Checkmark;
				break;
		}
	}

    void DisplayMedal(float time)
    {
        if(time < platinumTime)
		{
			FinalMedal.sprite = MedalSprites[4];
			MedalText.text = "Platinum";
		}
		else if(time < goldTime) {
            FinalMedal.sprite = MedalSprites[3];
            MedalText.text = "Gold";
        }
		else if(time < silverTime)
		{
            FinalMedal.sprite = MedalSprites[2];
            MedalText.text = "Silver";
        }
		else if(time < bronzeTime)
		{
            FinalMedal.sprite = MedalSprites[1];
            MedalText.text = "Gold";
        }
		else
		{
            FinalMedal.sprite = MedalSprites[0];
            MedalText.text = "None";
        }

    }

}
