using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { Idle, Following, Attacking, Hurting, Dying }
public class SeaMonster : EntityBase
{
    private readonly static int scaleRangeFactor = 6;

    public MonsterState state;
    public Sprite[] sprites;

    [Tooltip("Rotates the forward direction to be looking downwards. This is useful" +
        " when we want a sprite to look like its swimming. Still should maybe change.")]
    public bool bellyToOcean;

    [Header("Attack Stats")]
    [Range(1, 10)]  public int closeDMG;
    [Range(1, 10)]  public int rangeDMG;
    [Range(1, 4)]   public int closeRange;
    [Range(5, 10)]  public int longRange;

    static Transform player;
   
    Rigidbody rB;
    int atkRange, atkType;  

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        rB = GetComponent<Rigidbody>();

        CalculateNextAttack();

        if (bellyToOcean)
            sR.transform.Rotate(Vector3.right * 90, Space.Self);
    }

    private void Update()
    {
        if (state.Equals(MonsterState.Following))
            Follow();
    }

    public void Follow()
    {
        transform.LookAt(player);
        rB.MovePosition(Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed));

        if (Vector3.Distance(transform.position, player.position) <= atkRange * scaleRangeFactor)
        {
            rB.isKinematic = true;
            sR.sprite = sprites[atkType];
            if (bellyToOcean)
                sR.transform.Rotate(Vector3.left * 90, Space.Self);

            SwitchState(MonsterState.Attacking);
        }
    }

    private void CalculateNextAttack()
    {
        atkType = Mathf.CeilToInt(Random.value * 2) - 1;    // Returns either 0 or 1.
        atkRange = closeRange + (longRange * atkType);      // Defines the range for the next attack.
    }

    public void SwitchState(MonsterState mS) { this.state = mS; }
}
