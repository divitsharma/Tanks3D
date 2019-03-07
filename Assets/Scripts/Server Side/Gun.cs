using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour {

    [SerializeField]
    protected int ammo;

    public void TryFire()
    {
        if (ammo > 0)
        {
            Fire();
        }
    }

    abstract protected void Fire();
}
