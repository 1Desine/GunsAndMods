using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock_17 : Gun {


    private void Awake() {
        fireRate = 600;
        clipSize = 14;
        clipAmmo = 14;

        AddFireMod(FireModNames.Auto, false); // auto
        AddFireMod(FireModNames.Single, true); // single
    }


    private void OnEnable() {
        SetValuesInGunController();


        updateFunctions = () => {
            animator.SetBool(TRIGGER_BOOL, shootTriggerIsPulled);
        };
    }



    protected override bool CanShoot() {
        return shellIsLoaded;
    }
    protected override void Shoot() {
        float shootDistance = 100;

        Debug.DrawRay(barrelEndPoint.position, barrelEndPoint.forward * shootDistance, Color.red, 0.05f);

        animator.SetTrigger(SHOOT_TRIGGER);
    }
    protected override void ManageAmmoAfterShot() {
        if(clipAmmo > 0) {
            clipAmmo--;
        } else {
            shellIsLoaded = false;
        }
    }

    protected override void Reload() {
        clipAmmo = clipSize;

        if(shellIsLoaded == false) {
            clipAmmo--;
            shellIsLoaded = true;
        }
    }
}
