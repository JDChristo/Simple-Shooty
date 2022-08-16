using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Common;
using SimpleShooty.Game;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Weapon pistol;
    [SerializeField] private Weapon machineGun;
    [SerializeField] private Weapon shotGun;
    [SerializeField] private Weapon roketLauncher;

    private Enemy target;
    private Weapon currentWeapon;

    private void LateUpdate()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Start || currentWeapon == null)
        {
            return;
        }
        if (target == null || (target != null && !target.IsAlive))
        {
            target = enemyManager.GetClosetEnemy(currentWeapon.ShootingRange);
        }

        player.LookAt(target);
        if (target != null && target.IsAlive)
        {
            Fire();
        }
    }
    public void SelectWeapon(WeaponType weaponType)
    {
        DisableAllGuns();
        switch (weaponType)
        {
            case WeaponType.PISTOL:
                currentWeapon = pistol;
                break;
            case WeaponType.MACHINEGUN:
                currentWeapon = machineGun;
                break;
            case WeaponType.SHOTGUN:
                currentWeapon = shotGun;
                break;
            case WeaponType.ROCKETLAUNCHER:
                currentWeapon = roketLauncher;
                break;
            default:
                currentWeapon = pistol;
                break;
        }

        currentWeapon.Enable();
    }
    public void DisableAllGuns()
    {
        pistol.Disable();
        machineGun.Disable();
        shotGun.Disable();
        roketLauncher.Disable();
    }
    public void Fire()
    {
        currentWeapon.Fire(transform.forward, transform.rotation);
    }
}
