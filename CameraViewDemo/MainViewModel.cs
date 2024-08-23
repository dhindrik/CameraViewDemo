using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CameraViewDemo;

public partial class MainViewModel : ObservableObject
{
    private bool? flashOn;

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

    [ObservableProperty]
    private bool canRotateCamera;

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
    private void RetakePicture()
    {
        Bytes = null;
    }

    [RelayCommand]
    private void SavePicture() { }

    public Action<bool?>? SetFlashMode { get; set; }
    public Action? RotateCamera { get; set; }
}
