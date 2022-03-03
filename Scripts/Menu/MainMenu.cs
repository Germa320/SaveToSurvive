using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool levelChoice = false;
    bool controlsLevel1View = false;
    bool controlsLevel2View = false;
    public GameObject levelView;
    public GameObject mainMenu;
    public GameObject controlsLevel1;
    public GameObject controlsLevel2;
    LevelsAvailability availability;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        availability = GetComponent<LevelsAvailability>();
        Time.timeScale = 1;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
    }

    public void LoadLevelView()
    {
        levelChoice = true;
        levelView.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void Update()
    {
        if(levelChoice && Input.GetKeyDown(KeyCode.Escape))
        {
            levelView.SetActive(false);
            mainMenu.SetActive(true);
            levelChoice = false;
        }

        if (controlsLevel1View && Input.GetKeyDown(KeyCode.Escape))
        {
            controlsLevel1.SetActive(false);
            mainMenu.SetActive(true);
            controlsLevel1View = false;
        }

        if (controlsLevel2View && Input.GetKeyDown(KeyCode.Escape))
        {
            controlsLevel2.SetActive(false);
            mainMenu.SetActive(true);
            controlsLevel2View = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        if (availability != null)
        {
            if (availability.data.level1 && !availability.data.level2)
            {
                Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Level2Scene", LoadSceneMode.Single);
            }
        }
    }

    public void PlayLevel1()
    {
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }

    public void PlayLevel2()
    {
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level2Scene", LoadSceneMode.Single);
    }
}
