using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected WeaponProjectile bulletPrefab;
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float fireRange;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletLifeTime;
    [SerializeField] protected float bulletDamage;



    protected Queue<WeaponProjectile> bulletPool;
    protected int shootTrigger;
    protected bool canShoot;

    public float ShootingRange => fireRange;

    protected void Start()
    {
        bulletPool = new Queue<WeaponProjectile>();
        shootTrigger = Animator.StringToHash("Shoot");
        canShoot = true;
    }
    protected WeaponProjectile GetBullet()
    {
        if (bulletPool == null)
        {
            bulletPool = new Queue<WeaponProjectile>();
        }

        if (bulletPool.Count == 0)
        {
            var bullet = Instantiate(bulletPrefab).GetComponent<WeaponProjectile>();
            bullet.SetWeapon(this);
            return bullet;
        }
        else
        {
            var bullet = bulletPool.Dequeue();
            return bullet;
        }

    }
    protected void CleanPool()
    {
        if (bulletPool == null) { return; }
        foreach (var bullet in bulletPool)
        {
            Destroy(bullet);
        }
        bulletPool.Clear();
    }
    public void ReturnToPool(WeaponProjectile projectile)
    {
        bulletPool.Enqueue(projectile);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void Enable()
    {
        gameObject.SetActive(true);
        canShoot = true;
    }
    public virtual void Fire(Vector3 direction, Quaternion rotation)
    {
        if (!canShoot)
        {
            return;
        }
        var bullet = GetBullet();
        bullet.Shoot(spawnPoint.position, rotation, direction * bulletSpeed, bulletLifeTime);

        if (anim)
        {
            anim.SetTrigger(shootTrigger);
        }

        canShoot = false;
        StartCoroutine(WaitForNext());
    }

    public float GetDamage()
    {
        return bulletDamage;
    }
    protected IEnumerator WaitForNext()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
