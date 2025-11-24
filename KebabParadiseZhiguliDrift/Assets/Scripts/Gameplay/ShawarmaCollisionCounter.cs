using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShawarmaCollisionCounter : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("shawarma"))
        {
            _uiManager.CollectedShawarma++;
            Destroy(other.gameObject, 0.5f);
        }
    }
}
