using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour {

    [SerializeField] protected GunControllerSO gunControllerSO;

    [SerializeField] protected Transform barrelEndPoint;
    // [SerializeField] protected Transform defaultScopePosition;
    // [SerializeField] protected Transform attachedScopePosition;

    [SerializeField] protected List<ShootingMode> shootingModesAvailable;
    protected enum ShootingMode {
        Auto,
        Single,
        Burst,
    }


    protected int fireRate;
    protected int clipSize;

    protected int clipAmmo;
    protected bool shellIsLoaded;

    protected float lastShotTime;


    private void OnEnable() {
        SetValuesInGunController();
    }

    private void SetValuesInGunController() {
        gunControllerSO.shootButtonDown = false;
        gunControllerSO.reloadButtonDown = false;

        gunControllerSO.ClipAmmoChanged(clipAmmo);
        gunControllerSO.ClipSizeSet(clipSize);

        // gunControllerSO.DefaultScopePositionSet(defaultScopePosition.position);
        // gunControllerSO.ScopeMountPositionSet(attachedScopePosition.position);
    }

    private void Update() {
        if (gunControllerSO.shootButtonDown) TryShoot();
        if (gunControllerSO.reloadButtonDown) TryReload();
    }


    private void TryShoot() {
        if (CanShoot() == false) return;

        lastShotTime = Time.time;
        
        Shoot();

        ManageAmmoAfterShot();
        gunControllerSO.ClipAmmoChanged(clipAmmo);
    }

    private void TryReload() {
        Reload();
        gunControllerSO.ClipAmmoChanged(clipAmmo);
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






}
