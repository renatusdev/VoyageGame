using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCannon : Cannon
{
    public NormalCannon()
    {
        base.dmg = 5;
        base.gui = Resources.Load<Sprite>("Cannons/GUI/Normal Cannon");
    }

    public override void Fire()
    {
        base.Fire();
        Debug.Log("Fire Normal Cannon!");
    }
}
