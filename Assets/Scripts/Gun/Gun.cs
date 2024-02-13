using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    //this action is the delegate(a reference type variable that holds a reference to a method) that observers will subscribe to.
    public static Action OnShoot;
    
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = 0.4f;

    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire"); //fire animations' string name turned to int for performance
    private ObjectPool<Bullet> _bulletPool;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();   
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }
    private void OnEnable()
    {   
        OnShoot += ShootProjectile; 
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
    }
    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            OnShoot?.Invoke();// ? conditional operetor is used to prevent running into null exceptions.           
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);        
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = _mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x)* Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void CreateBulletPool ()
    {
       _bulletPool = new ObjectPool<Bullet>(() =>
       {
           return Instantiate(_bulletPrefab) ;
       },bullet =>
       {
           bullet.gameObject.SetActive(true);
       },bullet =>
       {
           bullet.gameObject.SetActive(false);
       },bullet =>
       {
           Destroy(bullet.gameObject);
       },false,20,40);          
    }

}
