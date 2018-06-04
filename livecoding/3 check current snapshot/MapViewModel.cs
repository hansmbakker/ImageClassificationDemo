//ADD ONNX as Content

private TurbineNoTurbineV2Model ModelGen;

public ICommand CheckCurrentWindowCommand => new DelegateCommand<object>(async (elementToRender) =>
        {
            var uiElement = (UIElement)elementToRender;
            TurbineNoTurbineV2ModelOutput modelOutput = await EvaluateMapControl(uiElement);

            var message = $"Turbine probability: {modelOutput.loss["turbine"]}\n"
                        + $"No turbine probability: {modelOutput.loss["no_turbine"]}";

            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        });

private async Task<TurbineNoTurbineV2ModelOutput> EvaluateMapControl(UIElement uiElement)
        {
            var videoFrame = await uiElement.RenderToVideoFrameAsync();
            var croppedVideoFrame = await videoFrame.CropVideoFrameAsync(256, 256);

            var modelInput = new TurbineNoTurbineV2ModelInput
            {
                data = croppedVideoFrame
            };
            var modelOutput = await ModelGen.EvaluateAsync(modelInput);
            return modelOutput;
        }


//CALL LoadModel in constructor

private async void LoadModel()
        {
            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/TurbineNoTurbineV2.onnx"));
            ModelGen = await TurbineNoTurbineV2Model.CreateTurbineNoTurbineV2Model(modelFile);
        }