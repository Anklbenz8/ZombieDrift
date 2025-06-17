using UnityEngine;

public static class InputUtils {
    public static bool CheckIfAnyKeyPressed(KeyCode[] keys) {
        foreach (var key in keys) {
            if (Input.GetKey(key))
                return true;
        }

        return false;
    }
}