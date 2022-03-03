using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelsData
{
    public bool level1;
    public bool level2;

    public LevelsData(Indestructable availability)
    {
        level1 = availability.level1;
        level2 = availability.level2;
    }
}
