using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    int GetDamage();
    int GetSenderID();
    void SetSenderID(int id);
}
