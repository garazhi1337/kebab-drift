using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KorobkaPeredachUI : MonoBehaviour
{
    [Header("передние и нейтралка")]
    [SerializeField] private Color _activeForwardColor;
    [SerializeField] private Color _passivForwardColor;
    [Header("задняя")]
    [SerializeField] private Color _activeBackwardColor;
    [SerializeField] private Color _passivBackwardColor;
    [Header("передачи текст")]
    [SerializeField] private TMP_Text[] _peredach;

    public void SetPeredachActive(int p)
    {
        for (int i = 0; i < _peredach.Length; i++)
        {
            if (i == 6) // 0 1 2 3 4 5 R=6 N=7
            {
                _peredach[i].color = _passivBackwardColor;
            }
            else
            {
                _peredach[i].color = _passivForwardColor;
            }
        }

        if (p == 6) // 0 1 2 3 4 5 R=6 N=7
        {
            _peredach[p].color = _activeBackwardColor;
        }
        else
        {
            _peredach[p].color = _activeForwardColor;
        }
    }
}
