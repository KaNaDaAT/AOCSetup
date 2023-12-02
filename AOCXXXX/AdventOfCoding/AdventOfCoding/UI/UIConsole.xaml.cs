using AdventOfCoding.Function.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
		public static UIConsole Instance
		{
			get {
				if (_instance == null)
				{
					_instance = new UIConsole();
				}
				return _instance;
			}
			private set { }
		}
		private Dispatcher dispatcher;

		public Dictionary<string, Command> Commands = new Dictionary<string, Command>();
		private readonly List<string> _commandHistory = new();
		private int _commandHistorySize = 100;
		private int _commandHistoryIndex = -1;
		private string _commandCurrent = "";

		public UIConsole()
		{
			InitializeComponent();
			_instance = this;

			tbConsole.TextChanged += readOnlyKeeper;
			tbConsole.KeyDown += OnKeyDownHandler;
			tbConsole.PreviewKeyDown += TbConsole_PreviewKeyDown;
		}

		public static void SetDispatcher(Dispatcher dispatcher)
		{
			Instance.dispatcher = dispatcher;
		}

		#region Write

		public static void Write(object output)
		{
			Write(output.ToString());
		}

		public static void Write(string output)
		{
			if (Instance.Dispatcher != null)
			{
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
			}
			else
			{
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

		public static void WriteLine(object output)
		{
			WriteLine(output.ToString());
		}

		public static void WriteLine(string output)
		{
			Write(output + Environment.NewLine);
		}

		public static void WriteLine(Exception exception)
			=> WriteError(exception);

		public static void WriteCommand(string command)
		{
			Instance.AddCommandToHistory(command);
			WriteLine(command);
		}

		public static void WriteError(Exception exception)
		{
			Write(exception.ToString() + Environment.NewLine);
			SwitchToCommandMode();
		}

		#endregion

		#region Text

		public static void SetText(string output)
		{
			Instance.isUserInput = false;
			Instance.tbConsole.Text = output;
			Instance.CurrentText = Instance.tbConsole.Text;
			Instance.CurrentLine = Instance.tbConsole.LineCount - 1;
			Instance.isUserInput = true;
		}

		public static void ClearText()
		{
			Instance.isUserInput = false;
			Instance.tbConsole.Text = "";
			Instance.CurrentText = Instance.tbConsole.Text;
			Instance.CurrentLine = 0;
			Instance.isUserInput = true;
		}

		private void UpdateConsoleText(bool force = false)
		{
			if (force
				|| !(
					tbConsole.Text.StartsWith(CurrentText) &&
					CurrentLine == tbConsole.GetLineIndexFromCharacterIndex(tbConsole.CaretIndex)
				)
				|| !InputEnabled
			)
			{
				tbConsole.Text = CurrentText + InputText;
				tbConsole.CaretIndex = tbConsole.Text.Length;
			}
		}

		#endregion

		#region Mode Swap

		public static void SwitchToCommandMode()
		{
			Instance.InputEnabled = true;
			Instance.tbConsole.IsReadOnly = false;
			if (!Instance.CurrentText.EndsWith(Environment.NewLine) && Instance.tbConsole.Text != "")
			{
				WriteLine("");
			}
			Write(Environment.CurrentDirectory + "> ");
			Instance.lbStatus.Content = UIConsoleState.Idle;
			Instance.lbStatus.UpdateLayout();
		}

		public static void SwitchToExecuteMode()
		{
			Instance.lbStatus.Content = UIConsoleState.Processing;
			Instance.lbStatus.UpdateLayout();
			Instance.InputEnabled = false;
			Instance.tbConsole.IsReadOnly = true;
		}

		#endregion

		#region Command

		public static void ExecuteCommand(string[] commandArgs)
		{
			SwitchToExecuteMode();
			string commandName = commandArgs[0];
			if (Instance.Commands.ContainsKey(commandName))
			{
				WriteLine("");
				Instance.Commands[commandName].Run(commandArgs);
			}
			SwitchToCommandMode();
		}

		private void CommandHistoryUp()
		{
			if (_commandHistoryIndex + 1 < _commandHistory.Count)
			{
				if (_commandHistoryIndex == -1)
				{
					_commandCurrent = InputText;
				}
				_commandHistoryIndex++;
				InputText = _commandHistory[_commandHistoryIndex];
				UpdateConsoleText(true);
			}
		}

		private void CommandHistoryDown()
		{
			if (_commandHistoryIndex > 0)
			{
				_commandHistoryIndex--;
				InputText = _commandHistory[_commandHistoryIndex];
				UpdateConsoleText(true);
			}
			else if (_commandHistoryIndex == 0)
			{
				_commandHistoryIndex = -1;
				InputText = _commandCurrent;
				UpdateConsoleText(true);
			}
			else
			{
				_commandHistoryIndex = -1;
			}
		}

		private void AddCommandToHistory(string command)
		{
			_commandHistory.Insert(0, command);
			if (_commandHistory.Count > _commandHistorySize)
			{
				_commandHistory.RemoveAt(_commandHistorySize);
			}
			_commandHistoryIndex = -1;
		}

		#endregion

		#region Event

		private void readOnlyKeeper(object sender, TextChangedEventArgs e)
		{
			if (!isUserInput)
			{
				return;
			}
			CurrentLine = tbConsole.LineCount - 1;

			if (tbConsole.CaretIndex >= CurrentText.Length)
			{
				InputText = tbConsole.Text.Substring(CurrentText.Length);
				Console.WriteLine(InputText);
			}


			UpdateConsoleText();
		}



		private void OnKeyDownHandler(object sender, KeyEventArgs e)
		{
			if ((e.Key == Key.Return || e.Key == Key.Enter) && tbConsole.CaretIndex >= CurrentText.Length)
			{
				AddCommandToHistory(InputText);
				try
				{
					ExecuteCommand(Regex.Split(InputText, "\\s(?=(?:[^\'\"`]*([\'\"`]).*?\\1)*[^\'\"`]*$)"));
				}
				catch (Exception ex)
				{
					WriteError(ex);
				}
				tbConsole.CaretIndex = tbConsole.Text.Length;
				InputText = "";
			}
		}

		private void TbConsole_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Down:
					if (tbConsole.CaretIndex < CurrentText.Length)
						return;
					CommandHistoryDown();
					e.Handled = true;
					return;
				case Key.Up:
					if (tbConsole.CaretIndex < CurrentText.Length)
						return;
					CommandHistoryUp();
					e.Handled = true;
					return;
				case Key.Left:
					if (tbConsole.CaretIndex == CurrentText.Length)
						e.Handled = true;
					return;
			}
		}

		private void bClear_Click(object sender, RoutedEventArgs e)
		{
			ClearText();
			SwitchToCommandMode();
		}

		#endregion
	}
}
