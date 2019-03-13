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

    int score = 0;
    public int Score
    {
        get { return score; }
    }

    [SerializeField]
    Material modelMaterial;

    public delegate void ScoreChangedDelegate(int connectionId, int newScore);
    public event ScoreChangedDelegate OnScoreChanged;

    public delegate void OnDeathDelegate(int connectionId, int killerId);
    OnDeathDelegate onDeath;

    // the unique ID of this player in the network - does it change if someone else disonnects?
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
            TakeDamage(damager.GetDamage(), damager.GetSenderID());
        }
    }

	// Use this for initialization
	void Start()
    {
        motor = GetComponent<PlayerMotor>();
        gun = GetComponent<BasicGun>();
        gun.AttachTo(this);
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

    public void SetColor(Color color)
    {
        MeshRenderer[] meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = color;
        }
    }

    public void IncrementScore()
    {
        score++;
        OnScoreChanged(connectionId, score);
    }

    void TakeDamage(int damage, int killerId)
    {
        health -= damage;
        if (health <= 0)
        {
            Die(killerId);
        }
    }

    void Die(int killerId)
    {
        onDeath(ConnectionId, killerId);
    }

}
