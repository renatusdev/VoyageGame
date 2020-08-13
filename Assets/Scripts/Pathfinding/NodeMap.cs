using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap : MonoBehaviour
{
    public static NodeMap instance;

    private List<Node> nodes;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        nodes = new List<Node>();

        foreach(Node n in transform.GetComponentsInChildren<Node>())
            nodes.Add(n);
    }

    public List<Node> GetNodes() { return nodes; }
}
