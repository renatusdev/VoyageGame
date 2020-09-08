using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Two purposes: Inform of resource amount, and select resource to use.
public class RingUIController : MonoBehaviour
{
    public static RingUIController instance;

    public GameObject ring;
    public GameObject dividers;
    public GameObject slots;

    private GameObject divider;
    private GameObject slot;
    private GameObject selectedSlot;

    private List<GameObject> divList;
    private List<GameObject> slotList;

    private int segments;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        divider = Resources.Load<GameObject>("GUI/Divider");
        slot = Resources.Load<GameObject>("GUI/Slot");

        slotList = new List<GameObject>();
        divList = new List<GameObject>();
    }

    public void Add(Item item)
    {
        // If slot already exists
        foreach(GameObject gO in slotList)
        {
            Slot slot = gO.GetComponent<Slot>();

            if (slot.GetItem().Equals(item))
            {
                slot.UpdateAmount();
                return;
            }
        }

        GameObject s = Instantiate(slot, slots.transform);
        
        s.GetComponent<Slot>().Instantiate(item);
        slotList.Add(s);

        AddSegments();
    }


    void AddSegments()
    {
        segments++;
        divList.Add(Instantiate(divider, dividers.transform));

        float anglePerSeg = 360 / segments;

        for (int i = 0; i < segments; i++)
        {
            float theta = (((i + 1) * anglePerSeg) + (i * anglePerSeg)) / 2;
            theta -= 90;
            theta *= Mathf.Deg2Rad;

            divList[i].LeanRotateZ(i * anglePerSeg - 90, 0.4f);
            slotList[i].LeanMoveLocal(new Vector3(300 * Mathf.Cos(theta), 300 * Mathf.Sin(theta)), 0.4f);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ring.SetActive(true);

            if (segments == 0)
                return;

            // Mouse Positions anchored to center screen.
            Vector2 mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;
            mousePos.Normalize(); ;

            // Mouse Angle starting starting from the bottom (-y axis)
            float angle = Mathf.Atan2(mousePos.y, mousePos.x);
            angle /= Mathf.PI;
            angle *= 180;
            angle += 90;

            if (angle < 0)
                angle += 360;

            float anglePerSeg = 360 / segments;

            for (int i = 0; i < segments; i++)
            {
                if (angle > i * anglePerSeg && angle < (i + 1) * anglePerSeg)
                {
                    GameObject s = slots.transform.GetChild(i).gameObject ;

                    if(selectedSlot == null || selectedSlot != s)
                    {
                        slots.transform.GetChild(i).GetComponentInChildren<TweenTool>().Tween();
                        selectedSlot = s;
                    }

                    if (Input.GetButtonDown("Fire1"))
                    {
                        // behaviour based off element chosen.
                        
                        // Find Item in Slot
                        // Use Item
                        // Remove Item amount
                    }
                }
            }
        }
        else
        {
            ring.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
