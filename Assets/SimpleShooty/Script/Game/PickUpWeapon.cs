using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Game;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public WeaponType WeaponType;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject machineGun;
    [SerializeField] private GameObject shotGun;
    [SerializeField] private GameObject roketLauncher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<CombatManager>().SelectWeapon(WeaponType);
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 6f).setLoopClamp();
        LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.4f).setLoopPingPong();

        pistol.SetActive(false);
        machineGun.SetActive(false);
        shotGun.SetActive(false);
        roketLauncher.SetActive(false);

        switch (WeaponType)
        {
            case WeaponType.PISTOL:
                pistol.SetActive(true);
                break;

            case WeaponType.MACHINEGUN:
                machineGun.SetActive(true);
                break;

            case WeaponType.SHOTGUN:
                shotGun.SetActive(true);
                break;

            case WeaponType.ROCKETLAUNCHER:
                roketLauncher.SetActive(true);
                break;

            default:
                pistol.SetActive(true);
                break;
        }
    }
    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
}
