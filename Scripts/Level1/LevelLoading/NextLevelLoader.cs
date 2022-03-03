using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class NextLevelLoader : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>();
    
    Bloom bloom;
    public PostProcessVolume volume;
    float bloomValue = 0f;
    AudioSource audioSource;
    public AudioClip winEnd;

    private void Start()
    {
        volume.profile.TryGetSettings(out bloom);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NextLevel())
        {
            StartCoroutine(IncreaseBloom());
            if (bloom.intensity.value >= 120f && Indestructable.instance.prevScene == "Level0Scene")
            {
                Indestructable.instance.level1 = true;
                SaveSystem.SaveLevels(Indestructable.instance);
                Time.timeScale = 1;
                Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Level0Scene", LoadSceneMode.Single);
            }
            else if(bloom.intensity.value >= 120f && Indestructable.instance.prevScene == "Menu")
            {
                Time.timeScale = 1;
                Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }

    bool NextLevel()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (!enemy.death)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator IncreaseBloom()
    {
        audioSource.PlayOneShot(winEnd);
        while (bloom.intensity.value <= 120f)
        {
            bloomValue += 0.05f;
            bloom.intensity.value = bloomValue;
            yield return null;
        }

    }
}
