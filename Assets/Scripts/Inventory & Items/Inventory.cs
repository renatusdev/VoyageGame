using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory i;

    public Wood wood;
    public Dictionary<CannonType, Cannon> cannons;

    private void Awake()
    {
        if (i == null)
            i = this;
        else
            Destroy(this);

        InstantiateItems();
    }

    void InstantiateItems()
    {
        wood = new Wood(0);

        cannons = new Dictionary<CannonType, Cannon>
        {
            { CannonType.Normal, new NormalCannon(0) },
            { CannonType.Anchor, new AnchorCannon(0) }
        };
    }

    private void Start()
    {
        AddItem(new Wood(2));
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            AddItem(new AnchorCannon(5));
        if (Input.GetKeyDown(KeyCode.U))
            AddItem(new NormalCannon(7));
    }

    public void AddItem(Item item)
    {
        if(item is Wood)
        {
            wood.AddAmount(item.amount);
        }
        else if(item is Cannon)
        {
            if (item is NormalCannon)
                cannons[CannonType.Normal].AddAmount(item.amount);
            if (item is AnchorCannon)
                cannons[CannonType.Anchor].AddAmount(item.amount);
        }

        RingUIController.instance.Add(item);
    }
}