using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCannon : Cannon
{
    public AnchorCannon(int amount)
    {
        base.type = ItemType.Cannons;
        base.dmg = 2;
        base.amount = amount;
        base.gui = Resources.Load<Sprite>("GUI/Cannons/Anchor Cannon");
    }

    public override void Fire()
    {
        base.Fire();
        //Debug.Log("Fire Anchor Cannon!");
    }
}
