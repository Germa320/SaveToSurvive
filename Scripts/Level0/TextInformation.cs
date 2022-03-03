using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextInformation : MonoBehaviour
{
    public Color backgroundColor;
    public Image backgroundImage;
    public Color textColor;

    bool visible = false;
    int secondsWait = 3;
    int secondsTillDark = 11;

    public TextAsset textBeforeFirstLevel;
    public TextAsset textBeforeSecondLevel;

    public TMPro.TMP_Text tmptoText;

    private void Update()
    {
        if(Time.timeSinceLevelLoad >= secondsWait && Time.timeSinceLevelLoad <= secondsTillDark && backgroundColor.a <= 1 && !visible)
        {
            backgroundColor.a += Time.deltaTime;
            backgroundImage.color = backgroundColor;
        }

        if (Time.timeSinceLevelLoad >= secondsTillDark && backgroundColor.a > 0)
        {
            backgroundColor.a -= Time.deltaTime;
            backgroundImage.color = backgroundColor;
            textColor.a -= Time.deltaTime;
            tmptoText.color = textColor;
            
        }

        if(Indestructable.instance.prevScene == "IntroScene")
        {
            tmptoText.text = textBeforeFirstLevel.text;
        }
        else
        {
            tmptoText.text = textBeforeSecondLevel.text;
        }
    }
}
