using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mainMenuCanvas;
    PlayerMovement playerMovement;
    PlayerMovementWithoutTimeManagment playerMovementWithoutTime;
    MouseLook mouseLook;
    public bool pause = false;

    public GameObject controlsLevel1;
    bool controlsLevel1View = false;
    public GameObject controlsLevel2;
    bool controlsLevel2View = false;

    bool gameStop = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        playerMovementWithoutTime = FindObjectOfType<PlayerMovementWithoutTimeManagment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pause && !controlsLevel1View && !controlsLevel2View)
        {
            ResumeGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !pause && !controlsLevel1View && !controlsLevel2View)
        {
            
            pause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (playerMovement != null) playerMovement.enabled = false;
            if (playerMovementWithoutTime != null) playerMovementWithoutTime.enabled = false;
            mouseLook.enabled = false;
            Time.timeScale = 0f;
            mainMenuCanvas.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pause && controlsLevel1View && !controlsLevel2View)
        {
            controlsLevel1.SetActive(false);
            mainMenu.SetActive(true);
            controlsLevel1View = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pause && !controlsLevel1View && controlsLevel2View)
        {
            controlsLevel2.SetActive(false);
            mainMenu.SetActive(true);
            controlsLevel2View = false;
        }
    }

    public void ResumeGame()
    {
        pause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerMovementWithoutTime != null) playerMovementWithoutTime.enabled = true;
        mainMenuCanvas.SetActive(false);
        mouseLook.enabled = true;
    }

    public void LoadLevel1ControlsView()
    {
        controlsLevel1View = true;
        controlsLevel1.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel2ControlsView()
    {
        controlsLevel2View = true;
        controlsLevel2.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Level0Scene")
        {
            ResumeGame();
            Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Level0Scene", LoadSceneMode.Single);
        }
        else if(sceneName == "Level1Scene")
        {
            ResumeGame();
            SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
            Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
        }
        else if(sceneName == "Level2Scene")
        {
            ResumeGame();
            SceneManager.LoadScene("Level2Scene", LoadSceneMode.Single);
            Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
        }
    }
}
