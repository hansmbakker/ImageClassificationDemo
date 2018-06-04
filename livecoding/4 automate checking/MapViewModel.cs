private MapControl _mapControl;

        private readonly IAutomaticLocationService _locationService;

public ICommand StartAutomaticCheckingCommand => new DelegateCommand<object>((mapControl) =>
       {
           _mapControl = mapControl as MapControl;

           if (_mapControl != null)
           {
               _locationService.StartListeningAsync();
           }
       });

        public ICommand StopAutomaticCheckingCommand => new DelegateCommand(async () =>
       {
           _locationService.StopListening();
           var initializationSuccessful = await _locationService.InitializeAsync();
       });



//INJECT IAutomaticLocationService in constructor as _locationService (modify)

private void AddMapIcon(Geopoint location, string title)
        {
            var mapIcon = new MapIcon()
            {
                Location = location,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title.GetLocalized(),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };
            MapIcons.Add(mapIcon);
        }

private async void LocationServicePositionChanged(object sender, Geopoint geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition;

                if (_mapControl != null)
                {
                    var modelOutput = await EvaluateMapControl(_mapControl);
                    if (modelOutput.loss["turbine"] > 0.7)
                    {
                        AddMapIcon(geoposition, "Turbine");
                    }
                }
            }
        }