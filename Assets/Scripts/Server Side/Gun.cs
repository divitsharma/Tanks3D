using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour {

    [SerializeField]
    protected int ammo;

    protected Player owner;

    // should call parent if overridden
    public void AttachTo(Player player)
    {
        owner = player;
    }

    public void TryFire()
    {
        if (ammo > 0)
        {
            Fire();
        }
    }

    abstract protected void Fire();
}
