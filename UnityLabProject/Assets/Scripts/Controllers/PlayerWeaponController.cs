using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponController", menuName = "WaponController/PlayerWeaponController")]
public class PlayerWeaponController : WeaponController
{
    public override bool RetrieveLeftClick()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool RetrieveLeftRelease()
    {
        return Input.GetMouseButtonUp(0);
    }
}
