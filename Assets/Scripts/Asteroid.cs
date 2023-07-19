using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;

    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManger _spawnManger;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManger = GameObject.Find("Spawn_Manager").GetComponent<SpawnManger>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(col.gameObject);
            _spawnManger.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }
}