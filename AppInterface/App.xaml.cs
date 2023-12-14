using AppInterface.Views;
namespace AppInterface;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
#if WINDOWS || IOS
        MainPage = new NavigationPage(new DesktopMainView());
#else
        MainPage = new NavigationPage(new DesktopMainView());
#endif
    }
}
