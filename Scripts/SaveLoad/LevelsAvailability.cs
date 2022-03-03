using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsAvailability : MonoBehaviour
{
    public LevelsData data;
    public Button level1;
    public Button level2;
    public Button continueGame;

    private void Start()
    {
        data = SaveSystem.LoadLevels();
        if(data != null)
        {
            if (data.level1)
            {
                level1.interactable = true;
            }

            if (data.level2)
            {
                level2.interactable = true;
            }

            if(data.level1 && !data.level2)
            {
                continueGame.interactable = true;
            }
            else
            {
                continueGame.interactable = false;
            }
        }
        else
        {
            continueGame.interactable = false;
        }
    }
}
