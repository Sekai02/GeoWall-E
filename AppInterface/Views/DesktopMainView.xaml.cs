using AppInterface.ViewModels;
using System.Linq.Expressions;

namespace AppInterface.Views;

public partial class DesktopMainView : ContentPage
{
	public DesktopMainView()
	{
		InitializeComponent();
		BindingContext = new DesktopMainViewModel();
	}
}