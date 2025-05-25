namespace RacingGameDemo.Editor.Gameplay.Cars
{
    using UnityEditor;
    
    using GameBoxSdk.Editor.Database;
    
    using RacingGameDemo.Runtime.Gameplay.Car;

    [CustomEditor(typeof(CarsDatabase))]
    public class CarsDatabaseInspector : GenerateDatabaseButtonInspector<CarsDatabase, CarDetails>
    {
        
    }
}

