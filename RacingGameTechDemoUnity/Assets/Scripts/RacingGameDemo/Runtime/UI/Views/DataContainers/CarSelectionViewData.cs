namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using RacingGameDemo.Runtime.Gameplay.Car;

    public class CarSelectionViewData : ViewInjectableData
    {
        public CarsDatabase CarsDatabase { get; private set; } = null;
        public string LastCarIdSelected { get; private set; } = null;

        public CarSelectionViewData(CarsDatabase carsDatabase, string lastCarIdSelected)
        {
            CarsDatabase = carsDatabase;
            LastCarIdSelected = lastCarIdSelected;
        }
    }
}

