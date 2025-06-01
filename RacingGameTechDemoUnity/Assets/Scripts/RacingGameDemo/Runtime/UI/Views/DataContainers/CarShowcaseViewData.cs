namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    using RacingGameDemo.Runtime.Gameplay.Car;

    public class CarShowcaseViewData : ViewInjectableData
    {
        public CarsDatabase CarsDatabase = null;

        public CarShowcaseViewData(CarsDatabase carsDatabase)
        {
            CarsDatabase = carsDatabase;
        }
    }
}

