using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishes : MonoBehaviour
{
    private readonly static int spawnDistance = 25;

    Transform player;
    
    void Start()
    {
        // Send current time to shader for dithering fade in. 
        GetComponentInChildren<MeshRenderer>().sharedMaterial.SetFloat("_StartupTime", Time.time);

        Instantiate();  
        SwimTowardsShip();
        JumpFishes();
    }


    void JumpFishes() 
    {
        // Randomly select a set of fishes
        // Give these a "Jumping Fish" parent class
        // Begin a coroutine for these jumping fish.
            // Lerp up and down
            // Rotate around x axis

        
    }



    // Spawn point behind ship
    void Instantiate()
    {
        player = GameAssets.i.player;
        transform.position = player.position + player.forward * - spawnDistance; 
    }

    private void Update () {
        SwimTowardsShip();
    }

    void SwimTowardsShip()
    {
        float t = Time.deltaTime / 0.02f;

        transform.position = Vector3.MoveTowards(transform.position, player.position, t * t * (3f - 2f * t));
        transform.forward = new Vector3(player.forward.x, transform.forward.y, player.forward.z);
    }


    private void OnDrawGizmos() 
    {
        if(Application.isPlaying)
            Gizmos.DrawSphere(player.position + player.forward * - spawnDistance, 5);
    }
}