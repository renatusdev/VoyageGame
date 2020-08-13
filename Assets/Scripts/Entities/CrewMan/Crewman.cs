using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class Crewman : MonoBehaviour
{
    Pathfinding pF;

    void Start()
    {
        pF = GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            pF.Move(NodeMap.instance.GetNodes()[Random.Range(0, NodeMap.instance.GetNodes().Count - 1)]);
    }
}
