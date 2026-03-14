using UnityEngine;
using UnityEngine.InputSystem;

namespace Karbon {

public sealed class CompositePaletteSelector : MonoBehaviour
{
    [System.Serializable]
    public struct Palette
    {
        public Color BGColor;
        public Color FGColor;
    }

    [SerializeField] CompositeController _target = null;
    [SerializeField] Palette[] _presets = new Palette[10];

    void Update()
    {
        var index = GetRequestedPresetIndex();
        if (index < 0) return;

        var p = _presets[index % _presets.Length];
        _target.BGColor = p.BGColor;
        _target.FGColor = p.FGColor;
    }

    static int GetRequestedPresetIndex()
    {
        var keyboard = Keyboard.current;
        if (keyboard.qKey.wasPressedThisFrame) return 0;
        if (keyboard.wKey.wasPressedThisFrame) return 1;
        if (keyboard.eKey.wasPressedThisFrame) return 2;
        if (keyboard.rKey.wasPressedThisFrame) return 3;
        if (keyboard.tKey.wasPressedThisFrame) return 4;
        if (keyboard.yKey.wasPressedThisFrame) return 5;
        if (keyboard.uKey.wasPressedThisFrame) return 6;
        if (keyboard.iKey.wasPressedThisFrame) return 7;
        if (keyboard.oKey.wasPressedThisFrame) return 8;
        if (keyboard.pKey.wasPressedThisFrame) return 9;
        return -1;
    }
}

} // namespace Karbon
