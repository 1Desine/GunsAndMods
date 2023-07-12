using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class GunControllerSO : ScriptableObject {

    [Header("Input to gun")]
    public bool shootButtonDown;
    public bool reloadButtonDown;
    public bool changeFiringModeButtonDown;



    // Output for use
    public event Action<int> OnClipAmmoChanged;
    public event Action<int> OnClipSizeSet; 
    public event Action<int> OnFireRateSet;
    public event Action<Gun.FireModNames> OnFiringModeChanged;

    // public event Action<Vector3> OnDefaultScopePositionSet;
    // public event Action<Vector3> OnScopeMountPositionSet;



    public void FireRateSet(int fireRate) {
        OnFireRateSet?.Invoke(fireRate);
    }
    public void ClipAmmoChanged(int clipAmmo) {
        OnClipAmmoChanged?.Invoke(clipAmmo);
    }
    public void ClipSizeSet(int clipSize) {
        OnClipSizeSet?.Invoke(clipSize);
    }
    public void FireModeChanged(Gun.FireModNames fireModName) {
        OnFiringModeChanged?.Invoke(fireModName);
    }


    // public void DefaultScopePositionSet(Vector3 defaultScopePosition) {
    //     OnDefaultScopePositionSet?.Invoke(defaultScopePosition);
    // }
    // public void ScopeMountPositionSet(Vector3 scopeMountPosition) {
    //     OnScopeMountPositionSet?.Invoke(scopeMountPosition);
    // }



}
