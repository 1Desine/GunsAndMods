using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunTestUI : MonoBehaviour {

    [SerializeField] private GunControllerSO gunControllerSO;

    [SerializeField] private TextMeshProUGUI firaRateText;
    [SerializeField] private TextMeshProUGUI clipAmmoText;
    [SerializeField] private TextMeshProUGUI clipSizeText;
    [SerializeField] private TextMeshProUGUI fireModText;

    [SerializeField] private Button shootButton;
    [SerializeField] private Button reloadButton;


    private void OnEnable() {
        gunControllerSO.OnClipSizeSet += GunControllerSO_OnClipSizeSet;
        gunControllerSO.OnClipAmmoChanged += GunControllerSO_OnClipAmmoChanged;
        gunControllerSO.OnFiringModeChanged += GunControllerSO_OnFiringModeChanged;
        gunControllerSO.OnFireRateSet += GunControllerSO_OnFireRateSet;
    }



    private void GunControllerSO_OnFireRateSet(int obj) {
        firaRateText.text = obj.ToString();
    }
    private void GunControllerSO_OnClipSizeSet(int obj) {
        clipAmmoText.text = obj.ToString();
    }
    private void GunControllerSO_OnClipAmmoChanged(int obj) {
        clipSizeText.text = obj.ToString();
    }
    private void GunControllerSO_OnFiringModeChanged(Gun.FireModNames obj) {
        fireModText.text = obj.ToString();
    }







}
