using System.Threading.Tasks;
using System.Windows;

namespace AdventOfCoding {
	/// <summary>
	/// Interaction.Function.for App.xaml
	/// </summary>
	public partial class App : Application {
		public static readonly int YEAR = 0000;

		public App()
		{
			Dispatcher.UnhandledException += Dispatcher_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
		}

		private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			UIConsole.WriteLine(e.Exception);
			e.Handled = true;
		}

		private static void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			UIConsole.WriteLine(e.Exception);
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			UIConsole.WriteLine(e.Exception);
		}
	}
}
