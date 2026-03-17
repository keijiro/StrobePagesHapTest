using Klak.TestTools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

namespace Karbon {

public sealed class SourceSelector : MonoBehaviour
{
    [SerializeField] ImageSource _imageSource = null;

    [CreateProperty] public List<string> SourceList => GetCachedSourceList();

    const string PrefKey = "VideoSourceName";

    List<string> GetCachedSourceList()
      => WebCamTexture.devices.Select(device => device.name).ToList();

    VisualElement UIRoot
      => GetComponent<UIDocument>().rootVisualElement;

    DropdownField UISelector
      => UIRoot.Q<DropdownField>("source-selector");

    void SelectSource(string name)
    {
        _imageSource.SourceName = name;
        _imageSource.SourceType = ImageSourceType.Webcam;
        PlayerPrefs.SetString(PrefKey, name);
    }

    void Start()
    {
        UISelector.dataSource = this;
        UISelector.RegisterValueChangedCallback(evt => SelectSource(evt.newValue));

        /*
        if (PlayerPrefs.HasKey(PrefKey))
        {
            var saved = PlayerPrefs.GetString(PrefKey);
            if (SourceList.Contains(saved)) SelectSource(UISelector.value = saved);
        }
        */
    }
}

} // namespace Karbon
