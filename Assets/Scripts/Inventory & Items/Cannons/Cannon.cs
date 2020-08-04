using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonType { Normal, Anchor, Egg }

[System.Serializable]
public class Cannon : Item 
{
    #region Fields
    public int dmg;
    #endregion

    public virtual void Fire() 
    {
        if(GetAmount() <= 0)
        {
            Debug.Log("No Cannons Left Captn'!");
        }
        amount--;
    }

    public int GetDMG() { return dmg; }
}
