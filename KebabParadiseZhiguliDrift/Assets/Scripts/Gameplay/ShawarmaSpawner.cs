using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineShawarmaSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _shawarmaPrefab;
    [SerializeField] private MeshCollider _roadCollider;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnHeight = 2f;
    private Coroutine _spawnCoroutine;

    void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    void OnDestroy()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            
            SpawnShawarma();
        }
    }

    private void SpawnShawarma()
    {
        Vector3 spawnPosition = GetGuaranteedSpawnPosition();
        
        if (spawnPosition != Vector3.zero)
        {
            GameObject shawarma = Instantiate(_shawarmaPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetGuaranteedSpawnPosition()
    {
        if (_roadCollider == null) return Vector3.zero;

        Bounds bounds = _roadCollider.bounds;
        
        // Пробуем разные точки пока не найдем валидную
        for (int i = 0; i < 200; i++)
        {
            Vector3 testPoint = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                bounds.max.y + _spawnHeight,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            RaycastHit hit;
            if (Physics.Raycast(testPoint, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider == _roadCollider)
                {
                    return hit.point + Vector3.up * _spawnHeight;
                }
            }
        }

        // Если не нашли случайную точку, используем центр
        return new Vector3(bounds.center.x, bounds.center.y + _spawnHeight, bounds.center.z);
    }
}