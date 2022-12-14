using AdventOfCoding.Function.Command;
using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AdventOfCoding {
	public partial class UIConsole : UserControl {
		// ToDo: Use https://www.codeproject.com/Articles/42490/Using-AvalonEdit-WPF-Text-Editor

		private string CurrentText = "";
		private string InputText = "";
		private int CurrentLine = 0;
		private bool isUserInput = true;

		public bool InputEnabled = false;

		private static UIConsole _instance = null;
		public static UIConsole Instance {
			get {
				if (_instance == null) {
					_instance = new UIConsole();
				}
				return _instance;
			}
			private set { }
		}

		private Dispatcher dispatcher;

		public Dictionary<string, Command> commands = new Dictionary<string, Command>();

		public UIConsole() {
			InitializeComponent();
			_instance = this;

			tbConsole.TextChanged += readOnlyKeeper;
			tbConsole.KeyDown += OnKeyDownHandler;
		}

		public static void SetDispatcher(Dispatcher dispatcher) {
			Instance.dispatcher = dispatcher;
		}

		public static void Write(object output) {
			Write(output.ToString());
		}

		public static void Write(string output) {
			if (Instance.Dispatcher != null) {
				Instance.Dispatcher.Invoke(() => {
					Instance.lbStatus.Content = UIConsoleState.Processing;
					Instance.lbStatus.UpdateLayout();
					Instance.isUserInput = false;
					Instance.tbConsole.AppendText(output);
					Instance.CurrentText = Instance.tbConsole.Text;
					Instance.CurrentLine = Instance.tbConsole.Text.Count(f => f == '\n');
					Instance.isUserInput = true;
					Instance.tbConsole.ScrollToVerticalOffset(double.MaxValue);
					Instance.lbStatus.Content = UIConsoleState.Idle;
					Instance.lbStatus.UpdateLayout();
				});
			} else {
				Instance.lbStatus.Content = UIConsoleState.Processing;
				Instance.isUserInput = false;
				Instance.tbConsole.AppendText(output);
				Instance.CurrentText = Instance.tbConsole.Text;
				Instance.CurrentLine = Instance.tbConsole.Text.Count(f => f == '\n');
				Instance.isUserInput = true;
				Instance.lbStatus.Content = UIConsoleState.Idle;
				Instance.lbStatus.UpdateLayout();
			}
		}

		public static void WriteLine(object output) {
			WriteLine(output.ToString());
		}

		public static void WriteLine(string output) {
			Write(output + Environment.NewLine);
		}

		public static void SetText(string output) {
			Instance.isUserInput = false;
			Instance.tbConsole.Text = output;
			Instance.CurrentText = Instance.tbConsole.Text;
			Instance.CurrentLine = Instance.tbConsole.LineCount - 1;
			Instance.isUserInput = true;
		}

		public static void ClearText() {
			Instance.isUserInput = false;
			Instance.tbConsole.Text = "";
			Instance.CurrentText = Instance.tbConsole.Text;
			Instance.CurrentLine = 0;
			Instance.isUserInput = true;
		}

		public static void SwitchToCommandMode() {
			Instance.InputEnabled = true;
			Instance.tbConsole.IsReadOnly = false;
			if (!Instance.CurrentText.EndsWith(Environment.NewLine) && Instance.tbConsole.Text != "") {
				WriteLine("");
			}
			Write(Environment.CurrentDirectory + "> ");
			Instance.lbStatus.Content = UIConsoleState.Idle;
			Instance.lbStatus.UpdateLayout();
		}

		public static void SwitchToExecuteMode() {
			Instance.lbStatus.Content = UIConsoleState.Processing;
			Instance.lbStatus.UpdateLayout();
			Instance.InputEnabled = false;
			Instance.tbConsole.IsReadOnly = true;
		}

		public static void ExecuteCommand(string[] commandArgs) {
			SwitchToExecuteMode();
			string commandName = commandArgs[0];
			if (Instance.commands.ContainsKey(commandName)) {
				WriteLine("");
				Instance.commands[commandName].Run(commandArgs);
			}
			SwitchToCommandMode();
		}

		#region Event

		private void readOnlyKeeper(object sender, TextChangedEventArgs e) {
			if (!isUserInput) {
				return;
			}
			CurrentLine = tbConsole.LineCount - 1;

			if (tbConsole.CaretIndex >= CurrentText.Length) {
				InputText = tbConsole.Text.Substring(CurrentText.Length);
				Console.WriteLine(InputText);
			}

			if (!(
				tbConsole.Text.StartsWith(CurrentText) &&
				CurrentLine == tbConsole.GetLineIndexFromCharacterIndex(tbConsole.CaretIndex)
			) || !InputEnabled) {
				tbConsole.Text = CurrentText + InputText;
				tbConsole.CaretIndex = tbConsole.Text.Length;
			}
		}



		private void OnKeyDownHandler(object sender, KeyEventArgs e) {
			if ((e.Key == Key.Return || e.Key == Key.Enter) && tbConsole.CaretIndex >= CurrentText.Length) {
				ExecuteCommand(Regex.Split(InputText, "\\s(?=(?:[^\'\"`]*([\'\"`]).*?\\1)*[^\'\"`]*$)"));
			}
		}

		private void bClear_Click(object sender, RoutedEventArgs e) {
			if (!InputEnabled)
				return;
			ClearText();
			SwitchToCommandMode();
		}

		#endregion
	}
}
