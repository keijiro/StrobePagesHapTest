using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Karbon {

[AddComponentMenu("VFX/Property Binders/Pad Input Value Binder")]
[VFXBinder("Karbon/Pad Input Value")]
public sealed class VFXPadInputValueBinder : VFXBinderBase
{
    [VFXPropertyBinding("System.Single")]
    public ExposedProperty Property = "FloatParameter";

    public PadInputHandler Source = null;

    public override bool IsValid(VisualEffect component)
      => Source != null && component.HasFloat(Property);

    public override void UpdateBinding(VisualEffect component)
      => component.SetFloat(Property, Source.Value);

    public override string ToString()
      => $"Pad Input Value : '{Property}' -> " +
         (Source != null ? Source.name : "(null)");
}

} // namespace Karbon
