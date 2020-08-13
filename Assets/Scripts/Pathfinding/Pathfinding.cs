using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Node currNode;
    public Node endNode;

    public float moveSpeed, rotationSpeed;

    public bool isMoving;

    void Start()
    {
        isMoving = false;
    }

    public void Move(Node endNode)
    {
        if (currNode == endNode)
            return;

        this.endNode = endNode;

        isMoving = true;
        FindPath(currNode.pos, this.endNode.pos);
    }


    private void FixedUpdate()
    {
        if (!isMoving)
            return;

        transform.position = Vector3.MoveTowards(transform.position, currNode.pos, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(currNode.pos - transform.position, Vector3.up), Time.deltaTime * rotationSpeed);

        if (Vector2.Distance(transform.position, currNode.pos) < 0.1f)
        {
            Debug.Log("reached");
            if (currNode == endNode)
            {
                Debug.Log("finally");
                isMoving = false;
            }
            FindPath(currNode.pos, endNode.pos);
        }
    }

    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node leastFCostNode = currNode.edges[0].b;

        foreach(Edge e in currNode.edges)
        {
            if (e.b == null)
                continue;

            e.b.CalculateCosts(startPos, endPos);

            if (e.b.fCost < leastFCostNode.fCost)
                leastFCostNode = e.b;
        }

        currNode = leastFCostNode;
    }
}
