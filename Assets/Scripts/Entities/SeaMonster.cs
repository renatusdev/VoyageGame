using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { Idle, Following, Attacking, Recovering, Hurting, Dying }
public class SeaMonster : EntityBase
{
    private readonly static int worldFactor = 6;
    private readonly static int hideDistance = 200;
    private readonly static int recoverDistance = 100;
    private readonly static int cdIDLE = 5;

    public MonsterState state;

    [Header("Entity Stats")]
    [Range(1, 10)]  public int closeATK;
    [Range(1, 10)]  public int farATK;
    [Range(4, 8)]   public int closeRNG;
    [Range(9, 15)]  public int farRNG;
    [Range(5, 15)]  public int HP;

    [Header("Prefabs used")]
    public GameObject monsterHitGPX;
    public GameObject shipHitGPX;

    static Transform player;

    Vector3 strafePos;
    Rigidbody rB;
    int rangeATK, typeATK;
    float timer;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        rB = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), this.GetComponent<Collider>());

        timer = cdIDLE;
        hp = HP;

        SwitchState(MonsterState.Idle);
        anim.SetInteger("typeATK", -1);

        CalculateStrafePos(300);
        CalculateNextATK();
    }

    private void FixedUpdate()
    {
        if (state.Equals(MonsterState.Idle))
            Idling();
        if (state.Equals(MonsterState.Following))
            Following();
        else if (state.Equals(MonsterState.Hurting))
            Hurting();
        else if (state.Equals(MonsterState.Recovering))
            Recovering();
    }

    void Idling()
    {
        if (Vector3.Distance(transform.position, strafePos) <= 1)
        {
            rB.velocity = Vector3.zero; // Not stopping the thing will #FirstNewton its way to the ship
            rB.angularVelocity = Vector3.zero;
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                CalculateStrafePos(300);
                timer = cdIDLE;
            }
        }
        else
        {
            transform.LookAt(strafePos);
            rB.MovePosition(Vector3.MoveTowards(transform.position, strafePos, Time.deltaTime * speed));
        }
    }

    void Following()
    {
        // Following
        transform.LookAt(player);
        rB.MovePosition(Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed));

        // Reached
        if (Vector3.Distance(transform.position, player.position) <= rangeATK * worldFactor)
        {
            // Not stopping the thing will #FirstNewton its way to the ship
            rB.velocity = Vector3.zero;
            rB.angularVelocity = Vector3.zero;

            anim.SetInteger("typeATK", typeATK);
            SwitchState(MonsterState.Attacking);
        }
    }

    public void Recover()
    {
        SwitchState(MonsterState.Recovering);

        CalculateNextATK();
        CalculateStrafePos(recoverDistance);
    }

    void Recovering()
    {
        transform.LookAt(strafePos);
        rB.MovePosition(Vector3.MoveTowards(transform.position, strafePos, Time.deltaTime * speed));

        if (Vector3.Distance(transform.position, strafePos) <= 1)
            SwitchState(MonsterState.Following);
    }

    public override void Hurting()
    {
        base.Hurting();

        transform.LookAt(strafePos);
        rB.MovePosition(Vector3.MoveTowards(transform.position, strafePos, Time.deltaTime * speed));

        if (Vector3.Distance(transform.position, strafePos) <= 1)
        {
            sR.color = Color.white;
            SwitchState(MonsterState.Following);
            anim.SetBool("isHurting", false);
        }
    }

    void OnParticleCollision(GameObject o)
    {
        if (o.CompareTag("Player") && !state.Equals(MonsterState.Hurting))
        {
            CalculateStrafePos(hideDistance);
            SwitchState(MonsterState.Hurting);
            Hurt(Inventory.i.cannons[CannonController.current].dmg);
            SoundManager.PlaySound(Sound.CannonHit, transform.position);

            // Particle Effect
            GameObject s = Instantiate(monsterHitGPX);
            s.transform.position = this.transform.position;
            s.GetComponent<ParticleSystem>().Play();
        }
    }

    void CalculateNextATK()
    {
        typeATK = Mathf.CeilToInt(Random.value * 2) - 1;    // Returns either 0 or 1.
        rangeATK = closeRNG + (farRNG * typeATK);           // Defines the range for the next attack.
    }

    void CalculateStrafePos(int distance)
    {
        strafePos = transform.position + (Random.insideUnitSphere * distance);
        strafePos = new Vector3(strafePos.x, -30, strafePos.z);
    }

    public void Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit))
        {
            GameObject gpx = Instantiate(shipHitGPX);
            gpx.transform.position = hit.point;
            gpx.transform.LookAt(this.transform);

            SoundManager.PlaySound(Sound.ShipHit, hit.point);

            player.GetComponent<Ship>().hp -= rangeATK == 0 ? closeATK : farATK; // Remove ship health
        }
        else
        {
            // Missed Attack
            SwitchState(MonsterState.Recovering);
            CalculateNextATK();
            CalculateStrafePos(recoverDistance);
        }

        anim.SetInteger("typeATK", -1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & !state.Equals(MonsterState.Recovering))
            SwitchState(MonsterState.Following);
    }

    public void SwitchState(MonsterState mS) { this.state = mS; }
}
