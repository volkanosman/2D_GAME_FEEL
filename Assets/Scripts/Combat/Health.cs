 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;

    public static Action<Health> OnDeath;

    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    


    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

   
}
