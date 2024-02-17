using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockBackThrust = 20f;

    private Vector2 _fireDirection;
    private Rigidbody2D _rigidBody;
    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Init(Gun gun,Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;       
    }


    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);

        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        knockback?.GetKnockedBack(PlayerController.Instance.transform.position , _knockBackThrust);

        _gun.ReleaseBulletFromPool(this);
    }
}