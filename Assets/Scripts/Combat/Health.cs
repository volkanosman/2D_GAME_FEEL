 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public Action onDeath;

    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    private void OnEnable()
    {
        onDeath += SpawnDeathSplatterPrefab;
        onDeath += SpawnDeathVFX;
    }
    private void OnDisable()
    {
        onDeath -= SpawnDeathSplatterPrefab;
        onDeath -= SpawnDeathVFX;
    }


    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, transform.rotation);
        SpriteRenderer deathSplatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;
        deathSplatterSpriteRenderer.color = currentColor;
     }

    private void SpawnDeathVFX()
    {
        GameObject deathVFX = Instantiate(_deathVFX, transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;
        ps.startColor = currentColor;
    }
}
