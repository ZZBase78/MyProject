using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void SetDamage(Vector3 form_position, Vector3 to_position, float damage);
}
