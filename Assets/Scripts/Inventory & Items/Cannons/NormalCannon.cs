using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCannon : Cannon
{
    public NormalCannon(int amount)
    {
        base.type = ItemType.Cannons;
        base.dmg = 5;
        base.amount = amount;
        base.gui = Resources.Load<Sprite>("GUI/Cannons/Normal Cannon");
    }

    public override void Fire()
    {
        base.Fire();
        //Debug.Log("Fire Normal Cannon!");
    }
}
