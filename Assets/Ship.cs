using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipState { Anchored, Sailing, Attacking, Sinking }
public abstract class Ship : MonoBehaviour
{
    [Header("Base Properties")]
    [Range(1, 100)] public int hp;
    [Range(0, 45)] public int speed;
    [Range(1, 40)] public int ram;  // TODO(Sergio): Apply this param once ram dmg is implemented 
    [Range(1, 20)] public int dmg; 
    public Gradient colorscheme;

    [SerializeField] 
    protected ShipState currState;
    protected Rigidbody rb;
    
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    protected abstract void RamShip();
    protected abstract void SteerHelm();
    protected abstract void TrimSails();

    protected virtual void Sink()
    {
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.y - 30,0,0), Time.deltaTime/5));
        rb.MovePosition(transform.position - Vector3.up * Time.deltaTime * 3);
    }

    protected virtual void OnHit(int dmg) 
    {
        hp -= dmg;

        if(hp <= 0)
        {
            Destroy(GetComponent<Collider>());
            SwitchState(ShipState.Sinking);
        }
            
    }

    private void OnParticleCollision(GameObject o) 
    {
        OnHit(o.transform.root.GetComponent<Ship>().dmg);
    }

    public ShipState GetState() { return currState; }

    protected void SwitchState(ShipState s){ currState = s; }
}