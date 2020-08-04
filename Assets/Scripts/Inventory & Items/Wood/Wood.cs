using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Item
{
    public Wood(int amount)
    {
        base.type = ItemType.Wood;
        base.gui = Resources.Load<Sprite>("GUI/Wood/Normal Wood");
        base.amount = amount;
    }


    public void AddWood(int amount) { base.amount += amount; }
}