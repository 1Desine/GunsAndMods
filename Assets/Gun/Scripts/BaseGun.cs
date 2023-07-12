using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour {

    [SerializeField] protected GunControllerSO gunControllerSO;

    [SerializeField] protected Transform barrelEndPoint;
    [SerializeField] protected Transform defaultScopePosition;
    [SerializeField] protected Transform attachedScopePosition;



    // Firing mode
    private bool changeFiringMode;
    protected int shootingModeSelected = 0;
    protected List<FiringMode> shootingModesList = new List<FiringMode>();
    protected class FiringMode {
        public bool singleAction;
        public int amountOfShots;
        public float nextShotDelay;
    }

    // Stats
    protected int fireRate = 60;
    protected int clipSize;

    protected int clipAmmo;
    protected bool shellIsLoaded;

    // Shooting
    private bool shootTriggerIsPulled;
    protected float lastShotTime;

    // Reload
    private bool reloadButtonIsPushedIn;



    private void OnEnable() {
        SetValuesInGunController();
    }


    private void Update() {
        ManageShooting(gunControllerSO.shootButtonDown);

        ManageReloading(gunControllerSO.reloadButtonDown);

        ManageFireModeChanging(gunControllerSO.changeFiringMode);
    }




    private void SetValuesInGunController() {
        gunControllerSO.shootButtonDown = false;
        gunControllerSO.reloadButtonDown = false;
        gunControllerSO.changeFiringMode = false;

        gunControllerSO.ClipAmmoChanged(clipAmmo);
        gunControllerSO.ClipSizeSet(clipSize);

        gunControllerSO.DefaultScopePositionSet(defaultScopePosition.position);
        gunControllerSO.ScopeMountPositionSet(attachedScopePosition.position);
    }



    private void ManageShooting(bool shootButtonDown) {
        if(shootButtonDown == false) {
            shootTriggerIsPulled = false;
            return;
        }
        FiringMode mode = shootingModesList[shootingModeSelected];

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
    private IEnumerator ShootingCoroutine(int amountOfShots, float nextShotDelay) {
        if(CanShoot()) Shoot();
        lastShotTime = Time.time;
        ManageAmmoAfterShot();
        gunControllerSO.ClipAmmoChanged(clipAmmo);

        amountOfShots--;
        if(amountOfShots > 0) {
            yield return new WaitForSeconds(nextShotDelay);
            StartCoroutine(ShootingCoroutine(amountOfShots, nextShotDelay));
        }
    }


    private void ManageFireModeChanging(bool changeFireMode) {
        if(changeFireMode && this.changeFiringMode == false) {
            shootingModeSelected++;
            shootingModeSelected %= shootingModesList.Count;
        }
        this.changeFiringMode = changeFireMode;
    }
    protected void AddFireMod(bool singleAction, int amountOfShots = 1, float nextShotDelay = -1) {
        shootingModesList.Add(new FiringMode() {
            singleAction = singleAction,
            amountOfShots = amountOfShots,
            nextShotDelay = nextShotDelay == -1 ? 60f / fireRate : nextShotDelay, // if burst speed same as auto - leave as default
        });
    }


    private void ManageReloading(bool reloadButtonDown) {
        if(reloadButtonDown && this.reloadButtonIsPushedIn == false) {
            Reload();
            gunControllerSO.ClipAmmoChanged(clipAmmo);
        }
        this.reloadButtonIsPushedIn = reloadButtonDown;
    }


    protected virtual bool CanShoot() {
        Debug.LogError("BaseGun CanShoot() called - reterned true");
        return true;
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





}
