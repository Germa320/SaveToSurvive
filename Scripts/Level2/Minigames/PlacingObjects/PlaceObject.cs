using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public List<GameObject> rocks = new List<GameObject>();
    public float itemDistance;
    public Material rockMaterial;
    public Material woodMaterial;
    public GameObject woodPile;
    Inventory inventory;
    int rocksPlaced = 0;
    bool woodPlaced = false;
    bool woodEquipped = true;

    AudioSource audioSource;
    public AudioClip objectPut;

    public Transform nextMapPoint1;
    public Transform nextMapPoint2;
    public Transform nextMapPoint3;

    public void ActivateRocks()
    {
        foreach(GameObject rock in rocks)
        {
            rock.SetActive(true);
        }
    }

    public void ActivateWoodPile()
    {
        woodPile.SetActive(true);
    }

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, itemDistance))
            {
                Debug.Log(hit.transform.name);

                if(hit.transform.tag == "TransparentRock")
                {
                    if (inventory.CheckIfSelected("Rock"))
                    {
                        rocksPlaced++;
                        audioSource.PlayOneShot(objectPut);
                        hit.transform.GetComponent<MeshRenderer>().material = rockMaterial;
                        hit.transform.tag = "Untagged";
                        inventory.RemoveItem(inventory.mItems[ScrollWheelHandler.Instance.index], false);
                    }
                    
                }

                if (hit.transform.tag == "TransparentWoodPile")
                {
                    if (inventory.CheckIfSelected("WoodPile"))
                    {
                        woodPlaced = true;
                        foreach(Transform woodPiece in woodPile.transform)
                        {
                            woodPiece.GetComponent<MeshRenderer>().material = woodMaterial;
                        }

                        hit.transform.tag = "Untagged";
                        audioSource.PlayOneShot(objectPut);
                        inventory.RemoveItem(inventory.mItems[ScrollWheelHandler.Instance.index], false);
                    }

                }
            }
        }

        if (inventory.CheckIfContains("WoodPile", 1) && woodEquipped)
        {
            MissionPointer.instance.ChangeTarget(nextMapPoint1);
            QuestManager.instance.QuestChange(GetComponent<Quest>().index + 1);
            woodEquipped = false;
        }

        if (rocksPlaced == 13)
        {
            MissionPointer.instance.ChangeTarget(nextMapPoint1);
            QuestManager.instance.QuestChange(GetComponent<Quest>().index);
            rocksPlaced = 0;
        }

        if (woodPlaced)
        {
            MissionPointer.instance.ChangeTarget(nextMapPoint2);
            QuestManager.instance.QuestChange(GetComponent<Quest>().index + 2);
            woodPlaced = false;
        }
    }
}
