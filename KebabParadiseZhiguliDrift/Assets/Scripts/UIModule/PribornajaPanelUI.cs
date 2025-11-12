using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PribornajaPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _obMinTMP;
    [SerializeField] private TMP_Text _kmHTMP;

    private int _obMin;
    public int ObMin
    {
        get => _obMin;
        set
        {
            if (_obMin != value)
            {
                _obMin = value;
                _obMinTMP.text = Convert.ToString(_obMin);
            }
        }
    }
    
    private int _kmH;
    public int KmH
    {
        get => _kmH;
        set
        {
            if (_kmH != value)
            {
                _kmH = value;
                _kmHTMP.text = Convert.ToString(_kmH);
            }
        }
    }
}
