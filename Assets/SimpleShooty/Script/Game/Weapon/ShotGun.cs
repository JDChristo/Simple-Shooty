using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    [SerializeField] private float spread;
    public override void Fire(Vector3 direction, Quaternion rotation)
    {
        if (!canShoot)
        {
            return;
        }
        for (int i = -3; i <= 3; i++)
        {
            var finalRot = rotation.eulerAngles;
            finalRot.y += (i * spread);

            var bullet = GetBullet();
            var finalDirection = Quaternion.AngleAxis(i * spread, Vector3.up) * direction;

            bullet.Shoot(spawnPoint.position, Quaternion.Euler(finalRot), finalDirection * bulletSpeed, bulletLifeTime);
        }
        if (anim)
        {
            anim.SetTrigger(shootTrigger);
        }

        canShoot = false;
        StartCoroutine(WaitForNext());
    }
}
