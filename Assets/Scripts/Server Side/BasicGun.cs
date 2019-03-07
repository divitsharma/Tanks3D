using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : Gun {

    [SerializeField]
    Transform nozzleTransform;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletLifetime = 5f;

    // seconds between bullets
    float fireRate = 0.3f;
    float sSinceFired = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (sSinceFired < fireRate)
            sSinceFired += Time.deltaTime;
	}

    // to be called when there is enough ammo
    protected override void Fire()
    {
		if (sSinceFired >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, nozzleTransform.position, nozzleTransform.rotation);
            ammo--;
            Destroy(bullet, bulletLifetime);
            Invoke("RestoreAmmo", bulletLifetime);
            sSinceFired = 0f;
        }


    }

    void RestoreAmmo()
    {
        ammo++;
    }
}
