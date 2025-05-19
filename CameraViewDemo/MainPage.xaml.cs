using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace CameraViewDemo;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel viewModel;
    private readonly ICameraProvider cameraProvider;
    private const string _wideAngleDeviceIds = "com.apple.avfoundation.avcapturedevice.built-in_video:5";

    public MainPage(MainViewModel viewModel, ICameraProvider cameraProvider)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        this.cameraProvider = cameraProvider;
        BindingContext = viewModel;

       

        viewModel.SetFlashMode = SetFlashMode;
        viewModel.RotateCamera = RotateCamera;
        viewModel.ToggleWideAngle = ToggleWideAngle;
    }

    private CameraInfo selectedCamera;
    private bool isCameraInitialized;
    public CameraInfo SelectedCamera
    {
        get => selectedCamera;
        set
        {
            selectedCamera = value;
            OnPropertyChanged();

            if (!isCameraInitialized && cameraProvider.AvailableCameras is not null)
            {
                viewModel.CanRotateCamera = cameraProvider.AvailableCameras.Where(x => x.Position == CameraPosition.Front)!.Count() > 0;
            }

            viewModel.HasFlash = value.IsFlashSupported;
            viewModel.MinZoomLevel = value.MinimumZoomFactor;
            viewModel.MaxZoomLevel = value.MaximumZoomFactor;

            isCameraInitialized = true;
        }
    }

    private CameraFlashMode flashMode;
    public CameraFlashMode FlashMode
    {
        get => flashMode;
        set
        {
            flashMode = value;
            OnPropertyChanged();
        }
    }

    private void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
    {
        var memoryStream = new MemoryStream();
        e.Media.CopyTo(memoryStream);

        viewModel.Bytes = memoryStream.ToArray();
    }

    private void SetFlashMode(bool? flashOn)
    {
        FlashMode = flashOn switch
        {
            false => CameraFlashMode.Off,
            true => CameraFlashMode.On,
            _ => CameraFlashMode.Auto
        };
    }

    private void RotateCamera()
    {
        if (viewModel.CanRotateCamera)
        {
            if (SelectedCamera == cameraProvider.AvailableCameras[0])
            {
                SelectedCamera = cameraProvider.AvailableCameras.First(x => x.Position == CameraPosition.Front);
            }
            else
            {
                SelectedCamera = cameraProvider.AvailableCameras[0];
            }
        }
    }

    private void ToggleWideAngle()
    {
        if (SelectedCamera.DeviceId != _wideAngleDeviceIds)
        {
            SelectedCamera = cameraProvider.AvailableCameras!.First(x => x.DeviceId == _wideAngleDeviceIds);
        }
        else
        {
            SelectedCamera = cameraProvider.AvailableCameras![0];
        }
    }
}