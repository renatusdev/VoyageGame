using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cannon 
{
    #region Fields
    public int dmg;
    public int amount;
    public Sprite gui;
    #endregion

    public virtual void Fire() 
    {
        if(amount <= 0)
        {
            // TODO: Put SFX
            Debug.Log("No Cannons Left Captn'!");
        }
        amount--; 
    }

    public void AddAmount(int add) { amount+= add; }
    public int GetAmount() { return amount; }
    public int GetDMG() { return dmg; }
    public Sprite GetGUI() { return gui; }
}
