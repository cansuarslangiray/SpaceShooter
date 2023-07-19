using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupId;
    [SerializeField]
    private AudioClip _audio;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
            if (col.CompareTag("Player"))
            {
                Player player = col.transform.GetComponent<Player>();
                AudioSource.PlayClipAtPoint(_audio,transform.position);
                if (player != null)
                {
                    if (_powerupId == 0)
                    {
                        player.TripleShotActive();

                    }
                    else if (_powerupId == 1)
                    {
                        player.SpeedBoostActive();
                    }
                    else if(_powerupId==2)
                    {
                        player.ShieldActive();
                    }
                }

                Destroy(this.gameObject);
            }
        
    }
}