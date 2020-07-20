using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCannon : Cannon
{

    public AnchorCannon()
    {
        base.dmg = 2;
        base.gui = Resources.Load<Sprite>("Cannons/GUI/Anchor Cannon");
    }

    public override void Fire()
    {
        base.Fire();
        Debug.Log("Fire Anchor Cannon!");
    }
}
