namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    public class MessageWindowViewData : ViewInjectableData
    {
        public string Message { get; private set; } = string.Empty;
        public bool DisplayCancelButton { get; private set; } = false;

        public MessageWindowViewData(string message, bool displayCancelButton)
        {
            Message = message;
            DisplayCancelButton = displayCancelButton; 
        }
    }
}

