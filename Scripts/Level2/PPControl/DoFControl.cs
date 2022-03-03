using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class DoFControl : MonoBehaviour
{
    Ray raycast;
    RaycastHit hit;
    public bool stopAdjusting = false;
    float hitDistance;

    public PostProcessVolume volume;

    DepthOfField depthOfField;
    Vignette vignette;

    private void Start()
    {
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        if (!stopAdjusting)
        {
            raycast = new Ray(transform.position, transform.forward * 100);


            if (Physics.Raycast(raycast, out hit, 8))
            {
                hitDistance = Vector3.Distance(transform.position, hit.point);
            }
            else
            {
                if (hitDistance < 8f)
                {
                    hitDistance++;
                }
            }
        }
        else
        {
            hitDistance = 1.64f;
        }

        if (QuestManager.instance.questsComplited)
        {
            vignette.intensity.value += Time.deltaTime * 0.3f;

            if(vignette.intensity.value >= 1 && (Indestructable.instance.prevScene == "Level0Scene" || (Indestructable.instance.prevScene == "Menu" && !Indestructable.instance.level2)))
            {
                Indestructable.instance.level2 = true;
                SaveSystem.SaveLevels(Indestructable.instance);
                Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("EndScene");
            }
            else if(vignette.intensity.value >= 1 && Indestructable.instance.prevScene == "Level0Scene" && Indestructable.instance.level2)
            {
                Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Menu");
            }
        }

        SetFocus();
    }

    void SetFocus()
    {
        depthOfField.focusDistance.value = hitDistance;
    }
}
