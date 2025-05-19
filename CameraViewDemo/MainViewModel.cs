using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CameraViewDemo;

public partial class MainViewModel : ObservableObject
{
    private bool? flashOn;
    private bool isWideMode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowPhoto))]
    [NotifyPropertyChangedFor(nameof(ShowCamera))]
    private byte[]? bytes;

    public bool ShowPhoto => Bytes is not null;
    public bool ShowCamera => Bytes is null;

    [ObservableProperty]
    private bool hasFlash;

    public string FlashIcon => flashOn switch
    {
        null => "flash_auto.png",
        false => "flash_off.png",
        _ => "flash_on.png"
    };

    public string WideIcon => isWideMode switch
    {
        true => "minimize.png",
        false => "expand.png"
    };

    [ObservableProperty]
    private bool canRotateCamera;

    [ObservableProperty]
    private float zoomLevel,maxZoomLevel, minZoomLevel;

    public CancellationToken Token => CancellationToken.None;

    [RelayCommand]
    private void Rotate() => RotateCamera?.Invoke();

    [RelayCommand]
    private void Flash()
    {
        flashOn = flashOn switch
        {
            false => true,
            true => null,
            _ => false
        };

        SetFlashMode?.Invoke(flashOn);

        OnPropertyChanged(nameof(FlashIcon));
    }

    [RelayCommand]
    private void WideAngle()
    {
        isWideMode = !isWideMode;

        OnPropertyChanged(nameof(WideIcon));

        ToggleWideAngle?.Invoke();
    }


    [RelayCommand]
    private void RetakePicture()
    {
        Bytes = null;
    }

    [RelayCommand]
    private void SavePicture() { }

    public Action<bool?>? SetFlashMode { get; set; }
    public Action? RotateCamera { get; set; }
    public Action? ToggleWideAngle { get; set; }

    partial void OnMinZoomLevelChanged(float oldValue, float newValue)
    {
        ZoomLevel = newValue;
    }

    partial void OnMaxZoomLevelChanged(float oldValue, float newValue)
    {
        ZoomLevel = newValue;
    }
}
