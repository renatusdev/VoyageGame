using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODOS(sergi):  
 * - Bow/Stern attack calculation
 * - Change Attack after a variable number of cannons hit.
 *    - might not work with Bow/Stern attack  
 * - Coroutine for distance checkings
 * - Raycasting for collision detection
 */
public class PirateShip : Ship
{
    # region Constant Variables
    private readonly static int minFollowDist = 500;
    private readonly static int minDistForNewTarget = 50;
    private readonly static int minFireDist = 200;
    private readonly static int obstacleCheckingRange = 150;

    private readonly static int sideAttackDistance = 50;
    private readonly static int timeForPrediction = 4;
    
    private readonly static int rotationSpeed = 10;

    private readonly static WaitForSeconds fireCD = new WaitForSeconds(1);
    private readonly static WaitForSeconds reloadCD = new WaitForSeconds(3);
    # endregion

    # region Public Variables
    public Transform player;
    # endregion

    # region Storage Variables    
    Vector3 target, atkSide;
    PirateCannons cannons;
    LayerMask playerMask;
    Rigidbody rbd;
    bool firing;
    # endregion

    private void Start()
    {
        // player = GameAssets.i.player;
        rbd = player.GetComponent<Rigidbody>();
        cannons = GetComponentInChildren<PirateCannons>();
        firing = false;
        playerMask = LayerMask.GetMask("Player");

        // Sets a new target position.
        Vector3 rnd = transform.position + Random.onUnitSphere * 100;
        target = new Vector3(rnd.x, transform.position.y, rnd.z);

        SwitchState(ShipState.Sailing);
        StartCoroutine(Checker());
    }

    private void FixedUpdate() 
    {    
        if(GetState().Equals(ShipState.Sailing))
        {
            SteerHelm();
        }
        else if(GetState().Equals(ShipState.Attacking))
        {
            Attacking();
            SteerHelm();
        }
        else if(GetState().Equals(ShipState.Anchored))
        {

        }
        else if(GetState().Equals(ShipState.Sinking))
        {
            Sink();
        }
        else
        {
            Debug.Log("Welcome to the rabbit hole.");
        }
    }

    private void Attacking()
    {
        // Predicted player position based off current position and velocity.
        Vector3 targetPosAtT = player.position + rbd.velocity * timeForPrediction;
        targetPosAtT += atkSide; // Side to attack
        target = targetPosAtT;

        if(!firing & PlayerInFireRange() & cannons.CanFire())
            StartCoroutine(FireCannons(Random.Range(3, 6)));
        return;

        bool PlayerInFireRange()  { return Vector3.Distance(transform.position, target) <= minFireDist; }

        IEnumerator FireCannons(int shots)
        {
            if(shots > 0)
            {
                firing = true;
                cannons.Fire(player.position);
                yield return fireCD;

                shots--;
                StartCoroutine(FireCannons(shots));
            }
            else
            {
                Debug.Log("RELOADING!");
                yield return reloadCD;
                Debug.Log("DONE RELOADING!");
                firing = false;
            }
        }
    }

    IEnumerator Checker() 
    {
        if(GetState().Equals(ShipState.Anchored))
        {
            if(CheckForPlayer())
                SetAttackTarget();
        }
        else if(GetState().Equals(ShipState.Sailing))
        {
            // What if ship is dodging already? or attacking already? 
                //  The only thing that should change the target is if there is an obstacle in the way.

            if(CheckForBow())
                SetDodgeTarget();
            else if(CheckForPlayer())
                SetAttackTarget();
            else// if(CheckForTarget())
                SetNewTarget();
        }
        else if(GetState().Equals(ShipState.Attacking))
        {
            if(CheckForBow())           // If there's something blocking forward movement
                SetDodgeTarget();       // Change the target position to a new one accordingly.
            else if(CheckForTarget())   // If near player (in the state of attacking, the target is the player)
                SetAttackTarget();      // Change the target position to a new one.
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(Checker());
        yield break;

        bool CheckForBow()      { return Physics.Raycast(transform.position, transform.forward, 150, ~playerMask) 
                                    ||  Physics.Raycast(transform.position, Quaternion.AngleAxis(15, Vector3.up) * transform.forward, obstacleCheckingRange, ~playerMask)
                                    ||  Physics.Raycast(transform.position, Quaternion.AngleAxis(-15, Vector3.up) * transform.forward, obstacleCheckingRange, ~playerMask); } // , ~playerMask);   }
        bool CheckForPlayer()   { return Vector3.Distance(transform.position, player.position) < minFollowDist; }
        bool CheckForTarget()   { return Vector3.Distance(transform.position, target) < minDistForNewTarget;    }

        void SetDodgeTarget()
        {
                SwitchState(ShipState.Sailing);
                int spread = 50;

                for(int i = -spread; i <= spread; i += 10)
                {
                    RaycastHit hit_1;
                    Vector3 dir = Quaternion.AngleAxis(i, Vector3.up) * transform.forward;

                    if(!Physics.Raycast(transform.position, dir, out hit_1, obstacleCheckingRange, ~playerMask))
                    {
                        Debug.Log("Found new path");
                        int distanceAhead = 50;
                        target = transform.position + dir * distanceAhead;
                        return;
                    }
                }
        }

        void SetAttackTarget() 
        {
            Debug.Log("Attacking");
            Quaternion rot = transform.rotation;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            float side = Mathf.Sign(Vector3.Dot(transform.forward, player.right));

            transform.rotation = rot;

            // Bow/Stern Attack
            if(Random.value <= 0.5f)
            {
                // TODO(sergi): If the distance is too much then we gotta swap to side attack.

                // 1 if its front (bow) attack or -1 if its back (stern attack).
                float z = Mathf.RoundToInt(Mathf.Clamp01((transform.forward - player.forward).z));
                z += z - 1;

                atkSide = new Vector3(player.right.x * side * 50, 0, -z * 50);
            }
            else
            {
                atkSide = player.right * -1 * side * sideAttackDistance;
            }

            cannons.Aim(Mathf.RoundToInt(side));
            SwitchState(ShipState.Attacking);
        }

        void SetNewTarget()
        {
            Vector3 rnd = transform.position + Random.onUnitSphere * 100;
            target = new Vector3(rnd.x, transform.position.y, rnd.z);
        }
    }

    protected override void SteerHelm()
    {
        target = new Vector3(target.x, transform.position.y, target.z); 
        Quaternion rot = Quaternion.RotateTowards(transform.rotation, 
                            Quaternion.LookRotation(target - transform.position),
                            Time.deltaTime * rotationSpeed);

        rb.MoveRotation(rot);
        rb.velocity = transform.forward * speed;
    }
    
    private void OnDrawGizmos() 
    {
        if(Application.isPlaying)
        {
            Gizmos.DrawSphere(target, 15);

            Gizmos.DrawRay(transform.position, transform.forward * 150);    
            Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(15, Vector3.up) * transform.forward * obstacleCheckingRange );
            Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-15, Vector3.up) * transform.forward * obstacleCheckingRange );
            
            // Vector3 dir = Quaternion.AngleAxis(15, Vector3.up) * transform.forward;
            // Gizmos.DrawSphere(transform.position + dir * 50, 4);
        }
    }

    // ! METHODS TODO ! //

    protected override void RamShip()
    {

    }

    protected override void TrimSails()
    {
        
    }
}