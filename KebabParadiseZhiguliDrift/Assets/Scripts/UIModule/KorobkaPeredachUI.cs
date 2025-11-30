using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KorobkaPeredachUI : MonoBehaviour
{
    [Header("Передние и нейтралка")]
    [SerializeField] private Color _activeForwardColor;
    [SerializeField] private Color _passivForwardColor;
    
    [Header("Задняя")]
    [SerializeField] private Color _activeBackwardColor;
    [SerializeField] private Color _passivBackwardColor;
    
    [Header("Передачи текст")]
    [SerializeField] private TMP_Text[] _peredach;

    public void SetPeredachActive(int p)
    {
        // Проверяем выход за границы массива
        if (p < 0 || p >= _peredach.Length)
        {
            Debug.LogError($"Неверный индекс передачи: {p}. Допустимые значения: 0-{_peredach.Length - 1}");
            return;
        }

        // Сбрасываем цвета всех передач
        for (int i = 0; i < _peredach.Length; i++)
        {
            if (_peredach[i] == null) continue;

            if (i == 6) // R - задняя передача
            {
                _peredach[i].color = _passivBackwardColor;
            }
            else // Все остальные передачи (1-5 и N)
            {
                _peredach[i].color = _passivForwardColor;
            }
        }

        // Устанавливаем цвет активной передачи
        if (_peredach[p] != null)
        {
            if (p == 6) // R - задняя передача
            {
                _peredach[p].color = _activeBackwardColor;
            }
            else // Передние передачи и нейтралка
            {
                _peredach[p].color = _activeForwardColor;
            }
        }
        else
        {
            Debug.LogError($"Элемент передачи с индексом {p} не назначен в инспекторе");
        }
    }
}