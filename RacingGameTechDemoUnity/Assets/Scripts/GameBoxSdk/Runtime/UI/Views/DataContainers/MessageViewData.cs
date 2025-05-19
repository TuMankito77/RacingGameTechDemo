namespace GameBoxSdk.Runtime.UI.Views.DataContainers
{
    public class MessageViewData : ViewInjectableData
    {
        public string Message { get; private set; } = string.Empty;

        public MessageViewData(string message)
        {
            Message = message;
        }
    }
}
