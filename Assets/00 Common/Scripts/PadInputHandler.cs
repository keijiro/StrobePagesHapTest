using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Karbon {

public sealed class PadInputHandler : MonoBehaviour
{
    [SerializeField] InputAction _action = null;
    [SerializeField] float _releaseTime = 0.5f;
    [SerializeField] UnityEvent<float> _onValueChanged = null;

    const float kHoldTime = 0.5f;

    float _currentValue;
    float _timer;
    float _maxStrength;
    float _attenuationRate;
    bool _wasPressed;

    void OnEnable() => _action?.Enable();
    void OnDisable() => _action?.Disable();

    void Update()
    {
        if (_action == null) return;

        var strength = _action.ReadValue<float>();
        var isPressed = strength > 0;

        if (isPressed)
        {
            if (!_wasPressed)
            {
                _timer = 0;
                _maxStrength = 0;
            }

            _timer += Time.deltaTime;
            _maxStrength = Mathf.Max(_maxStrength, strength);
            _currentValue = strength;
        }
        else
        {
            if (_wasPressed)
            {
                if (_timer < kHoldTime)
                {
                    // Case 1: Short press
                    _currentValue = 1.0f;
                    var duration = _releaseTime * _maxStrength;
                    _attenuationRate = (duration > 1e-5f) ? (1.0f / duration) : float.MaxValue;
                }
                else
                {
                    // Case 2: Long press
                    _attenuationRate = (_releaseTime > 1e-5f) ? (1.0f / _releaseTime) : float.MaxValue;
                }
            }

            if (_currentValue > 0)
            {
                _currentValue -= _attenuationRate * Time.deltaTime;
                if (_currentValue < 0) _currentValue = 0;
            }
        }

        _onValueChanged?.Invoke(_currentValue);
        _wasPressed = isPressed;
    }
}

} // namespace Karbon
