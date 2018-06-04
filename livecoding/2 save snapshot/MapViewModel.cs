public ICommand SnapCurrentWindowCommand => new DelegateCommand<object>(async (elementToRender) =>
        {
            var uiElement = (UIElement)elementToRender;
            var videoFrame = await uiElement.RenderToVideoFrameAsync();
            var croppedVideoFrame = await videoFrame.CropVideoFrameAsync(256, 256);
            await croppedVideoFrame.SaveToFileAsync();
        });