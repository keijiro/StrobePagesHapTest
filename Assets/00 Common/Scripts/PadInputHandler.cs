using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Karbon {

public sealed class PadInputHandler : MonoBehaviour
{
    [field:SerializeField] public float ReleaseTime { get; set; } = 0.5f;
    [field:SerializeField, Min(1)] public float DecayExponent { get; set; } = 2.0f;

    [Space, SerializeField] InputAction _input = null;
    [Space, SerializeField] UnityEvent<float> _valueTarget = null;

    public float Value { get; private set; }

    const float kHoldTime = 0.2f;

    float _strength, _decayRate;
    double _startTime;

    void OnEnable()
    {
        _input.started += OnStarted;
        _input.performed += OnPerformed;
        _input.canceled += OnCanceled;
        _input.Enable();
    }

    void OnDisable()
    {
        _input.started -= OnStarted;
        _input.performed -= OnPerformed;
        _input.canceled -= OnCanceled;
        _input.Disable();
    }

    void OnStarted(InputAction.CallbackContext context)
    {
        _startTime = context.time;
        _strength = context.ReadValue<float>();
    }

    void OnPerformed(InputAction.CallbackContext context)
      => _strength = Mathf.Max(_strength, context.ReadValue<float>());

    void OnCanceled(InputAction.CallbackContext context)
    {
        if (context.time - _startTime < kHoldTime)
        {
            // Short press: Decay from 1
            Value = 1;
            _decayRate = 1 / (ReleaseTime * Mathf.Pow(_strength, DecayExponent));
        }
        else
        {
            // Long press: Decay from current value
            _decayRate = 2 / ReleaseTime;
        }
    }

    void Update()
    {
        _valueTarget?.Invoke(Value);
        Value = Mathf.Max(_input.ReadValue<float>(), Value - _decayRate * Time.deltaTime);
    }
}

} // namespace Karbon
