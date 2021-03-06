﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour , IDamager {

    [SerializeField]
    float speed;

    [SerializeField]
    int damage;

    int senderId;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward.normalized * speed;
	}
	
    public int GetDamage()
    {
        return damage;
    }

    public int GetSenderID()
    {
        return senderId;
    }

    public void SetSenderID(int id)
    {
        senderId = id;
    }

}
