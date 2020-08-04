using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public TextMeshProUGUI amount;
    public Image gui;
    
    private Item item;

    public void Instantiate(Item item)
    {
        this.item = item;
        gui.sprite = item.GetGUI();
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        amount.SetText(item.GetAmount().ToString());
    }

    public Item GetItem() { return item; }
}
