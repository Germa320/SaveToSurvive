using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextWriter : MonoBehaviour
{
    public TMP_Text text;
    public TextAsset textAsset;
    public TextAsset textAsset2;
    string textToWrite;
    bool write = true;
    bool skip = false;
    string endText = "Well played.\nYou can now get back home.";
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "IntroScene") textToWrite = textAsset.text;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "IntroScene")
        {
            if (write)
            {
                StartCoroutine(TextWrite());
                write = false;
            }
            if (Input.GetKeyDown(KeyCode.Space)) skip = true;
        }

        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            if (write)
            {
                StartCoroutine(WriteEndText());
                write = false;
            }
        }
    }

    IEnumerator WriteEndText()
    {
        for (int i = 0; i < endText.Length; i++)
        {
            text.text += endText[i];
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
    }

    IEnumerator TextWrite()
    {
        for (int i = 0; i < textToWrite.Length; i++)
        {
            if (textToWrite[i] != '/') text.text += textToWrite[i];

            if (!skip)
            {
                if (textToWrite[i] == '\n')
                {
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else
            {
                if (textToWrite[i] == '/') skip = false;
                
            }
            yield return null;
            
        }
        skip = false;
        yield return new WaitForSeconds(1.2f);

        text.text = "";
        textToWrite = textAsset2.text;

        for (int i = 0; i < textToWrite.Length; i++)
        {
            if (textToWrite[i] != '/') text.text += textToWrite[i];

            if (!skip)
            {
                if (textToWrite[i] == '\n')
                {
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else
            {
                if (textToWrite[i] == '/') skip = false;

            }
            yield return null;

        }

        yield return new WaitForSeconds(1.2f);

        text.text = "";
        textToWrite = "You have to save to survive";

        for (int i = 0; i < textToWrite.Length; i++)
        {
            text.text += textToWrite[i];
            yield return new WaitForSeconds(0.03f);

        }
        yield return new WaitForSeconds(1f);
        int counter = 0;
        while (counter < 50)
        {
            text.enabled = false;
            yield return new WaitForSeconds(0.03f);
            text.enabled = true;
            yield return new WaitForSeconds(0.03f);
            counter++;
        }
        text.enabled = false;

        SceneManager.LoadScene("Level0Scene", LoadSceneMode.Single);
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
    }
}
