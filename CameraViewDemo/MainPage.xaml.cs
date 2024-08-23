using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace CameraViewDemo;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel viewModel;
    private readonly ICameraProvider cameraProvider;

    public MainPage(MainViewModel viewModel, ICameraProvider cameraProvider)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        this.cameraProvider = cameraProvider;
        BindingContext = viewModel;

        viewModel.CanRotateCamera = cameraProvider.AvailableCameras?.Count > 1;
        viewModel.SetFlashMode = SetFlashMode;
        viewModel.RotateCamera = RotateCamera;
    }

    private CameraInfo selectedCamera;
    public CameraInfo SelectedCamera
    {
        get => selectedCamera;
        set
        {
            selectedCamera = value;
            OnPropertyChanged();

            viewModel.HasFlash = value.IsFlashSupported;
            viewModel.MinZoomLevel = value.MinimumZoomFactor;
            viewModel.MaxZoomLevel = value.MaximumZoomFactor;            
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
        if (cameraProvider.AvailableCameras?.Count > 1)
        {
            if(SelectedCamera == cameraProvider.AvailableCameras[0])
            {
                SelectedCamera = cameraProvider.AvailableCameras[1];
            }
            else
            {
                SelectedCamera = cameraProvider.AvailableCameras[0];       
            }
        }
    }
}