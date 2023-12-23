using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimationEventListener : MonoBehaviour
{
    Fighter fighter;

    private void Awake()
    {
        fighter = GameObjectExtention.GetComponentAroundOrAdd<Fighter>(this.gameObject);
    }

    public void Hit()
    {
        fighter.Hit();
    }
}
