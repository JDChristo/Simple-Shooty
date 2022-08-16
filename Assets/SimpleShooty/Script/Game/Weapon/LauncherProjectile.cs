using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Game;
using UnityEngine;

public class LauncherProjectile : WeaponProjectile
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            foreach (var explosion in explosions)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            StopCoroutine(destoryCoroutine);
            rb.velocity = Vector3.zero;
            ExplosionForce();
        }
    }

    public void ExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (!collider.tag.Equals("Enemy"))
            {
                continue;
            }

            Rigidbody colBody = collider.gameObject.AddComponent<Rigidbody>();
            if (colBody != null)
            {
                colBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                colBody.GetComponent<Enemy>().ReceiveDamage(weapon.GetDamage());
            }
        }
        gameObject.SetActive(false);
        weapon.ReturnToPool(this);
        //
        //     gameObject.SetActive(false); 
        // other.GetComponent<Enemy>().ReceiveDamage(weapon.GetDamage());
    }
}
