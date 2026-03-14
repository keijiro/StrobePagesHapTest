using Klak.Hap;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Karbon {

public sealed class FlipbookSourceSelector : MonoBehaviour
{
    [SerializeField] string[] _videoFiles = null;
    [SerializeField] float _playbackSpeed = 10;
    [SerializeField] MeshRenderer _videoQuad = null;

    GameObject _playerObject;
    int _currentIndex;

    void Start()
      => RebuildPlayer(_currentIndex);

    void Update()
    {
        var index = GetRequestedMovieIndex();
        if (index < 0 || index == _currentIndex) return;
        RebuildPlayer(index);
    }

    void RebuildPlayer(int index)
    {
        if (_playerObject != null) Destroy(_playerObject);

        _playerObject = new GameObject("HAP Player");
        _playerObject.transform.SetParent(transform, false);

        var filePath = _videoFiles[index % _videoFiles.Length];

        var player = _playerObject.AddComponent<HapPlayer>();
        player.targetRenderer = _videoQuad;
        player.targetMaterialProperty = "_BaseMap";
        player.speed = _playbackSpeed;
        player.loop = true;
        player.Open(filePath, HapPlayer.PathMode.StreamingAssets);

        _currentIndex = index;
    }

    static int GetRequestedMovieIndex()
    {
        var keyboard = Keyboard.current;
        if (keyboard.digit1Key.wasPressedThisFrame) return 0;
        if (keyboard.digit2Key.wasPressedThisFrame) return 1;
        if (keyboard.digit3Key.wasPressedThisFrame) return 2;
        if (keyboard.digit4Key.wasPressedThisFrame) return 3;
        if (keyboard.digit5Key.wasPressedThisFrame) return 4;
        if (keyboard.digit6Key.wasPressedThisFrame) return 5;
        if (keyboard.digit7Key.wasPressedThisFrame) return 6;
        if (keyboard.digit8Key.wasPressedThisFrame) return 7;
        if (keyboard.digit9Key.wasPressedThisFrame) return 8;
        return -1;
    }
}

} // namespace Karbon
