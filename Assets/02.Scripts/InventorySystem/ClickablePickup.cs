using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickablePickup : MonoBehaviour, IRaycastable
{
    Pickup pickup;

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }

    public bool HandleRayCast(PlayerController callingController)
    {
        if(Input.GetMouseButtonDown(0))
        {
            pickup.PickupItem();
        }
        return true;
    }

    private void Awake()
    {
        pickup = GetComponent<Pickup>();
    }
}
