using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeDetector : MonoBehaviour
{

    public Action ledgeDetected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ledge")
        {
            ledgeDetected?.Invoke();
        }
    }

}
