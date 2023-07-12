using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Gun;

public class Gun : MonoBehaviour {

    [SerializeField] protected GunControllerSO gunControllerSO;

    [SerializeField] protected Transform barrelEndPoint;
    [SerializeField] protected Transform defaultScopePosition;
    [SerializeField] protected Transform attachedScopePosition;

    [SerializeField] protected Animator animator;

    protected const string TRIGGER_BOOL = "Trigger";
    protected const string SHOOT_TRIGGER = "Shoot";
    



    protected delegate void UpdateFunctions();
    protected UpdateFunctions updateFunctions;


    // Fire mode
    private bool changeFiringMode;
    protected int shootingModeSelected = 0;
    protected List<FireMode> shootingModesList = new List<FireMode>();
    protected class FireMode {
        public FireModNames fireMod;
        public bool singleAction;
        public int amountOfShots;
        public float burstFireRate;
    }
    public enum FireModNames {
        Auto,
        Single,
        Burst,
    }

    // Stats
    protected int fireRate = 60;
    protected int clipSize;

    protected int clipAmmo;
    protected bool shellIsLoaded = true;

    // Shooting
    protected bool shootTriggerIsPulled;
    protected float lastShotTime;

    // Reload
    private bool reloadButtonIsPushedIn;

    

    private void Update() {
        ManageShooting(gunControllerSO.shootButtonDown);

        ManageReloading(gunControllerSO.reloadButtonDown);

        ManageFireModeChanging(gunControllerSO.changeFiringModeButtonDown);

        updateFunctions();
    }




    protected void SetValuesInGunController() {
        gunControllerSO.shootButtonDown = false;
        gunControllerSO.reloadButtonDown = false;
        gunControllerSO.changeFiringModeButtonDown = false;

        gunControllerSO.FireRateSet(fireRate);
        gunControllerSO.ClipAmmoChanged(clipAmmo);
        gunControllerSO.ClipSizeSet(clipSize);
        gunControllerSO.FireModeChanged(shootingModesList[shootingModeSelected].fireMod);


        // gunControllerSO.DefaultScopePositionSet(defaultScopePosition.position);
        // gunControllerSO.ScopeMountPositionSet(attachedScopePosition.position);
    }


    private void ManageShooting(bool shootButtonDown) {
        if(shootButtonDown == false) {
            shootTriggerIsPulled = false;
            return;
        }
        FireMode mode = shootingModesList[shootingModeSelected];

        bool canShoot = false;
        if(Time.time - lastShotTime > 60f / fireRate) { // Rate of fire allows to shoot another round
            canShoot = true;
            if(mode.singleAction && shootTriggerIsPulled == true) { // Single action mode AND trigger was Not reset
                canShoot = false;
            }
        }
        if(canShoot) {
            StartCoroutine(ShootingCoroutine(mode.amountOfShots, mode.burstFireRate));
        }

        shootTriggerIsPulled = shootButtonDown;
    }
    private IEnumerator ShootingCoroutine(int amountOfShots, float burstFireRate) {
        if(CanShoot()) Shoot();
        lastShotTime = Time.time;
        ManageAmmoAfterShot();
        gunControllerSO.ClipAmmoChanged(clipAmmo);

        amountOfShots--;
        if(amountOfShots > 0) {
            yield return new WaitForSeconds(60f / burstFireRate);
            StartCoroutine(ShootingCoroutine(amountOfShots, burstFireRate));
        }
    }


    private void ManageFireModeChanging(bool changeFireMode) {
        if(changeFireMode && this.changeFiringMode == false) {
            shootingModeSelected++;
            shootingModeSelected %= shootingModesList.Count;
            gunControllerSO.FireModeChanged(shootingModesList[shootingModeSelected].fireMod);
        }
        this.changeFiringMode = changeFireMode;
    }
    protected void AddFireMod(FireModNames fireMod, bool singleAction, int amountOfShots = 1, float burstFireRate = -1) {
        shootingModesList.Add(new FireMode() {
            fireMod = fireMod,
            singleAction = singleAction,
            amountOfShots = amountOfShots,
            burstFireRate = burstFireRate == -1 ? 60f / fireRate : 60f / burstFireRate, // if burst speed same as auto - leave as default
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

        //if (Physics.Raycast(barrelEndPoint.position, barrelEndPoint.forward, out RaycastHit hit, shootDistance)) {
        //    Debug.DrawRay(barrelEndPoint.position, barrelEndPoint.forward * (hit.point - barrelEndPoint.position).magnitude, Color.red, 0.05f);
        //}
    }
    protected virtual void ManageAmmoAfterShot() {
        Debug.LogError("BaseGun ManageAmmoAfterShot() called");
    }

    protected virtual void Reload() {
        Debug.LogError("BaseGun Reload() called");
    }





}
