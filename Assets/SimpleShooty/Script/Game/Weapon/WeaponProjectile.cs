using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Game;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected GameObject[] explosions;
    protected Weapon weapon;
    protected Rigidbody rb;
    protected Coroutine destoryCoroutine;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            foreach (var explosion in explosions)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            StopCoroutine(destoryCoroutine);
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            weapon.ReturnToPool(this);

            other.GetComponent<Enemy>().ReceiveDamage(weapon.GetDamage());
        }
    }
    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Shoot(Vector3 position, Quaternion rotation, Vector3 direction, float bulletLifeTime)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        rb.AddForce(direction, ForceMode.Impulse);
        destoryCoroutine = StartCoroutine(DestroyBullet(bulletLifeTime));
    }

    protected virtual IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trail != null)
        {
            trail.Clear();
        }
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        weapon.ReturnToPool(this);

    }
}
