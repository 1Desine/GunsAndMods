using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour {

    [SerializeField] protected GunControllerSO gunControllerSO;

    [SerializeField] protected Transform barrelEndPoint;
    [SerializeField] protected Transform defaultScopePosition;
    [SerializeField] protected Transform attachedScopePosition;

    protected int shootingModeSelected = 0;
    protected List<ShootingMode> shootingModesList = new List<ShootingMode>();
    protected class ShootingMode {
        public bool singleAction;
        public int amountOfShots;
        public float nextShotDelay;
    }


    protected int fireRate = 300;
    protected int clipSize;

    protected int clipAmmo;
    protected bool shellIsLoaded;

    protected float lastShotTime;

    protected bool shootTriggerIsPulled;


    private void Awake() {
        shootingModesList.Add(new ShootingMode() {
            singleAction = true,
            amountOfShots = 3,
            nextShotDelay = 60f / fireRate,
        });
        Debug.Log(shootingModesList[0].singleAction);
        Debug.Log(shootingModesList[0].amountOfShots);
        Debug.Log(shootingModesList[0].nextShotDelay);
    }

    private void OnEnable() {
        SetValuesInGunController();
    }

    private void SetValuesInGunController() {
        gunControllerSO.shootButtonDown = false;
        gunControllerSO.reloadButtonDown = false;

        gunControllerSO.ClipAmmoChanged(clipAmmo);
        gunControllerSO.ClipSizeSet(clipSize);

        gunControllerSO.DefaultScopePositionSet(defaultScopePosition.position);
        gunControllerSO.ScopeMountPositionSet(attachedScopePosition.position);
    }

    private void Update() {
        ManageShooting(gunControllerSO.shootButtonDown);

        if(gunControllerSO.reloadButtonDown) TryReload();
    }


    private void ManageShooting(bool shootButtonDown) {
        if(shootButtonDown == false) {
            shootTriggerIsPulled = false;
            return;
        }
        ShootingMode mode = shootingModesList[shootingModeSelected];

        bool canShoot = false;
        if(Time.time - lastShotTime > 60f / fireRate) { // Rate of fire allows to shoot another round
            canShoot = true;
            if(mode.singleAction && shootTriggerIsPulled == true) { // Single action mode AND trigger was Not reset
                canShoot = false;
            }
        }
        if(canShoot) {
            StartCoroutine(ShootingCoroutine(mode.amountOfShots, mode.nextShotDelay));
        }

        shootTriggerIsPulled = shootButtonDown;
    }

    private void TryReload() {
        Reload();
        gunControllerSO.ClipAmmoChanged(clipAmmo);
    }


    private IEnumerator ShootingCoroutine(int amountOfShots, float nextShotDelay) {
        Shoot();
        lastShotTime = Time.time;
        ManageAmmoAfterShot();
        gunControllerSO.ClipAmmoChanged(clipAmmo);

        amountOfShots--;
        if(amountOfShots > 0) {
            yield return new WaitForSeconds(nextShotDelay);
            StartCoroutine(ShootingCoroutine(amountOfShots, nextShotDelay));
        }
    }



    protected virtual bool CanShoot() {
        Debug.LogError("BaseGun CanShoot() called");
        return false;
    }
    protected virtual void Shoot() {
        Debug.LogError("BaseGun Shoot() called");
    }
    protected virtual void ManageAmmoAfterShot() {
        Debug.LogError("BaseGun ManageAmmoAfterShot() called");
    }

    protected virtual void Reload() {
        Debug.LogError("BaseGun Reload() called");
    }


    protected void SelectNextShootingMode() {
        shootingModeSelected++;
        shootingModeSelected %= shootingModesList.Count;
    }



}
