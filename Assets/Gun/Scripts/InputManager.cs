using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public GunControllerSO gunControllerSO;

    public bool useKeys;
    public KeyCode shoot;
    public KeyCode reload;
    public KeyCode changeFireMode;
    public bool saveKeys;

    public KeyBindings keyBindings = new KeyBindings();
    public class KeyBindings {
        public KeyCode shoot;
        public KeyCode reload;
        public KeyCode changeFireMode;
    }

    private void Awake() {
        keyBindings = JsonUtility.FromJson<KeyBindings>(File.ReadAllText(Application.dataPath + "/keyBindings.json"));
        shoot = keyBindings.shoot;
        reload = keyBindings.reload;
        changeFireMode = keyBindings.changeFireMode;
    }

    private void Update() {
        if(useKeys) {
            gunControllerSO.shootButtonDown = Input.GetKey(keyBindings.shoot) ? true : false;
            gunControllerSO.reloadButtonDown = Input.GetKey(keyBindings.reload) ? true : false;
            gunControllerSO.changeFiringModeButtonDown = Input.GetKey(keyBindings.changeFireMode) ? true : false;
        }

        if(saveKeys) {
            keyBindings.shoot = shoot;
            keyBindings.reload = reload;
            keyBindings.changeFireMode = changeFireMode;
            string json = JsonUtility.ToJson(keyBindings);
            File.WriteAllText(Application.dataPath + "/keyBindings.json", json);
        }
    }
}
