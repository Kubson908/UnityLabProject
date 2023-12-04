using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : ScriptableObject
{
    public abstract bool RetrieveLeftClick();
    public abstract bool RetrieveLeftRelease();
}
