using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _colledtedShawarmaTMP;
    private float _colledtedShawarma;
    public float CollectedShawarma
    {
        get => _colledtedShawarma;
        set
        {
            if (_colledtedShawarma != value)
            {
                _colledtedShawarma = value;
                _colledtedShawarmaTMP.text = $"{_colledtedShawarma}";
            }
        }
    }
}
