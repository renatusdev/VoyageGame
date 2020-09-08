using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public bool followPlayer;

    void Start()
    {
      if(followPlayer)
        target = GameAssets.i.player;

        if (target == null)
            Debug.LogError(string.Format("[0] is has no target", name));
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
}
