using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(BasicGun))] // rn doing this, make guns prefabs later
public class Player : MonoBehaviour {

    PlayerMotor motor;
    Gun gun;

    [SerializeField]
    int maxHealth = 100;
    int health;

    public delegate void OnDeathDelegate(int connectionId);
    OnDeathDelegate onDeath;

    int connectionId;
    public int ConnectionId
    {
        get { return connectionId; }
        set { connectionId = value; }
    }

    void OnCollisionEnter(Collision other)
    {
        IDamager damager = other.gameObject.GetComponent<IDamager>();
        if (damager != null)
        {
            TakeDamage(damager.GetDamage());
        }
    }

	// Use this for initialization
	void Start()
    {
        motor = GetComponent<PlayerMotor>();
        gun = GetComponent<BasicGun>();
        health = maxHealth;
    }

    public void UpdateFromClient(ClientUpdateMessage msg)
    {
        if (msg.speedScale != 0)
        {
            motor.RotateTo(msg.rotateTo);
        }
        motor.AddVelocity(msg.speedScale);
        if (msg.firing) gun.TryFire();
    }

    public void AddOnDeathHandler(OnDeathDelegate del)
    {
        onDeath += del;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onDeath(ConnectionId);
    }

}
