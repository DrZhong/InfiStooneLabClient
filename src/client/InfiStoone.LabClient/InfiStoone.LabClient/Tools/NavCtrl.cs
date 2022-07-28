using GalaSoft.MvvmLight.Messaging;

namespace InfiStoone.LabClient.Tools
{
    public static class NavCtrl
    {
        public static void NavToPage(string pageName)
        {
            Messenger.Default.Send<string>($"Pages.{pageName}", MessengerToken.LoadShowContent);
        }
    }
}