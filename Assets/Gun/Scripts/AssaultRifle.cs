using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultRifle : BaseGun {

    

    private void Awake() {
        fireRate = 600;
        clipSize = 30;
        clipAmmo = 0;
        shellIsLoaded = false;
    }


    protected override bool CanShoot() {
        bool canShootNextShell = Time.time - lastShotTime > 60f / fireRate;
        return shellIsLoaded && canShootNextShell;
    }
    protected override void Shoot() {
        float shootDistance = 500;
        if (Physics.Raycast(barrelEndPoint.position, barrelEndPoint.forward, out RaycastHit hit, shootDistance)) {
            Debug.DrawRay(barrelEndPoint.position, barrelEndPoint.forward * (hit.point - barrelEndPoint.position).magnitude, Color.red, 0.05f);
        }
    }
    protected override void ManageAmmoAfterShot() {
        if (clipAmmo > 0) {
            clipAmmo--;
        }
        else {
            shellIsLoaded = false;
        }
    }

    protected override void Reload() {
        clipAmmo = clipSize;

        if (shellIsLoaded == false) {
            clipAmmo--;
            shellIsLoaded = true;
        }
    }


}
