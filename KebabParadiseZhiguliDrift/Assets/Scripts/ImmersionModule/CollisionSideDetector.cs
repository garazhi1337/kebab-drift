using System;
using System.Collections;
using System.Collections.Generic;
using Bhaptics.SDK2;
using UnityEngine;

public class CollisionSideDetector : MonoBehaviour
{
    [SerializeField] private Transform[] _sides; //0-лево верх 1-право верх 2-лево низ 3-право низ

    private void OnCollisionEnter(Collision other)
    {
        float[] distances = new float[_sides.Length];
        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Mathf.Abs(_sides[i].position.magnitude - other.transform.position.magnitude);
        }
        float shortest = Mathf.Min(distances);

        if (distances[0] == shortest)
        {
            BhapticsLibrary.Play(eventId: BhapticsEvent.LEFTHIT, startMillis: 0, intensity: 1, duration: 1, angleX: 0, offsetY: 0); //тактильный ивент
            Debug.Log("помеха слева");
        }
        
        if (distances[1] == shortest)
        {
            BhapticsLibrary.Play(eventId: BhapticsEvent.RIGHTHIT, startMillis: 0, intensity: 1, duration: 1, angleX: 0, offsetY: 0); //тактильный ивент
            Debug.Log("помеха справа");
        }
        
        if (distances[2] == shortest)
        {
            BhapticsLibrary.Play(eventId: BhapticsEvent.MINIMAL_CRASH_BACK, startMillis: 0, intensity: 1, duration: 1, angleX: 0, offsetY: 0); //тактильный ивент
            Debug.Log("помеха сзади");
        }
        
        if (distances[3] == shortest)
        {
            BhapticsLibrary.Play(eventId: BhapticsEvent.MINIMAL_CRASH_BACK, startMillis: 0, intensity: 1, duration: 1, angleX: 0, offsetY: 0); //тактильный ивент
            Debug.Log("помеха сзади");
        }
    }
}
