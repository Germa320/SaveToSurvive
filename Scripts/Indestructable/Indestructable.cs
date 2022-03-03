using UnityEngine;
using System.Collections;

public class Indestructable : MonoBehaviour
{
    public static Indestructable instance = null;
    public string prevScene = "null";
    public bool level1 = false;
    public bool level2 = false;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
