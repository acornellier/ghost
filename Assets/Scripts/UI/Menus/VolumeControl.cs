using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] AudioManager.AudioChannel channel = AudioManager.AudioChannel.Master;

    [Inject] AudioManager _audioManager;

    Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    void Start()
    {
        _slider.minValue = 0;
        _slider.maxValue = 1;
        _slider.value = AudioManager.GetChannelValue(channel);
    }

    void HandleSliderValueChanged(float value)
    {
        _audioManager.SetVolume(channel, value);
    }
}
