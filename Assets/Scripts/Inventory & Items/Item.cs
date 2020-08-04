using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int amount;
    public Sprite gui;
    public ItemType type;

    public void AddAmount(int add) { amount += add; }
    public int GetAmount() { return amount; }
    public Sprite GetGUI() { return gui; }
}
public enum ItemType { Wood, Cannons }