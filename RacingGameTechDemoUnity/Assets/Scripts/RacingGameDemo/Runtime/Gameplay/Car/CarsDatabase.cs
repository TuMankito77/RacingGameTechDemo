namespace RacingGameDemo.Runtime.Gameplay.Car
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Database;

    [CreateAssetMenu(fileName = CARS_DATABASE_ASSET_NAME, menuName = CARS_DATABASE_MENU_NAME)]
    public class CarsDatabase : FileDatabase<CarDetails>
    {
        public const string CARS_DATABASE_SCRIPTABLE_OBJECT_PATH = "RacingGameDemo/Cars/CarsDatabase";

        private const string CARS_DATABASE_ASSET_NAME = "CarsDatabase";
        private const string CARS_DATABASE_MENU_NAME = "Database/CarsDatabase";

        public override string FileDatabasePathScriptableObjectPath => CARS_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/RacingGameDemo/Editor/Database/Templates/TemplateCarIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/RacingGameDemo/Runtime/Gameplay/Car/CarIds.cs";
        protected override string TemplateIdVariableSlot => "#CarId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ",";
    }
}

