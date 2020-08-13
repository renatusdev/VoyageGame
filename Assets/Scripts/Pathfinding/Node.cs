using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private float gCost;   // Distance from starting node
    private float hCost;   // Distance from end node (Manhattan Distance)
    
    public float fCost { get { return gCost + hCost; } }
    public Vector3 pos { get { return transform.position; } }

    public Edge[] edges;

    private void Start()
    {
        if (edges.Length <= 0)
            Debug.Log(string.Format("Node {0} has no edges.", this.name));

        foreach (Edge e in edges)
            e.a = this;
    }

    public void CalculateCosts(Vector3 startNode, Vector3 endPos)
    {
        gCost = Vector3.Distance(transform.position, startNode);
        hCost = Vector3.Distance(transform.position, endPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);    

        Vector3 p = transform.position;

        try
        {
            UnityEditor.Handles.Label(new Vector3(p.x + 0.3f, p.y, p.z - 0.2f), string.Format("g: {0}", gCost.ToString("F2")));
            UnityEditor.Handles.Label(new Vector3(p.x + 0.3f, p.y, p.z + 0.2f), string.Format("h: {0}", hCost.ToString("F2")));
            UnityEditor.Handles.Label(new Vector3(p.x - 0.3f, p.y, p.z + 0.6f), string.Format("f: {0}", fCost.ToString("F2")));
            UnityEditor.Handles.Label(new Vector3(p.x, p.y, p.z - 0.6f), name);
        }
        catch
        {
            
        }

        if (edges.Length == 0)
            return;

        Gizmos.color = Color.grey;
        foreach (Edge e in edges)
        {
            if (e.b == null)
                continue;
            e.a = this;
            Gizmos.DrawLine(e.a.transform.position, e.b.transform.position);
        }
    }
}

[System.Serializable]
public class Edge
{
    public Node a;
    public Node b;
}