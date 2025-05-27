namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    using RacingGameDemo.Runtime.Gameplay.Car;

    public class CarSelectionViewData : ViewInjectableData
    {
        public CarsDatabase CarsDatabase { get; private set; }

        public CarSelectionViewData(CarsDatabase carsDatabase)
        {
            CarsDatabase = carsDatabase;
        }
    }
}

