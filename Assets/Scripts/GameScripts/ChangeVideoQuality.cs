using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVideoQuality : MonoBehaviour
{
    //[SerializeField] private Dropdown ChangeQuality;


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    public void SetQualityDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }

    public void SetFPSDropdown(int index)
    {
        switch (index)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;

            case 1:
                Application.targetFrameRate = 60;
                break;

            case 2:
                Application.targetFrameRate = 120;
                break;

            case 3:
                Application.targetFrameRate = 140;
                break;

            case 4:
                Application.targetFrameRate = -1;
                break;

            default:
                Application.targetFrameRate = 60;
                break;
        }
    }
}
