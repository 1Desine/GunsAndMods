using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GunTestUI : MonoBehaviour {

    [SerializeField] private GunControllerSO gunControllerSO;

    [SerializeField] private TextMeshProUGUI firaRateText;
    [SerializeField] private TextMeshProUGUI clipAmmoText;
    [SerializeField] private TextMeshProUGUI clipSizeText;


    private void OnEnable() {
        gunControllerSO.OnClipAmmoChanged += GunControllerSO_OnClipAmmoChanged;
        gunControllerSO.OnClipSizeSet += GunControllerSO_OnClipSizeSet;
    }

    private void OnDisable() {
        gunControllerSO.OnClipAmmoChanged -= GunControllerSO_OnClipAmmoChanged;
        gunControllerSO.OnClipSizeSet -= GunControllerSO_OnClipSizeSet;
    }

    private void GunControllerSO_OnClipSizeSet(int obj) {
        clipAmmoText.text = obj.ToString();
    }

    private void GunControllerSO_OnClipAmmoChanged(int obj) {
        clipSizeText.text = obj.ToString();
    }

    private void UpdateClipAmmoText() {
        
    }






}
