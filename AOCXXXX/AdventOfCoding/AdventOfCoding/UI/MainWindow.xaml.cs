using AdventOfCoding.Days;
using AdventOfCoding.Function.Command;
using Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdventOfCoding {
	/// <summary>
	/// Interaction.Function.for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window {

		private readonly List<(bool enabled, Button button)> AOCButtons = new List<(bool enabled, Button button)>();
		private readonly Dictionary<string, object> results = new Dictionary<string, object>();

		public MainWindow() {
			InitializeComponent();
			UIConsole.WriteLine($"AdventOfCoding{App.YEAR} Started...");
			UIConsole.SwitchToCommandMode();
			UIConsole.SetDispatcher(this.Dispatcher);

			CreateAdventButtons();

			UndefinedCommand commandOpen = new UndefinedCommand();
			commandOpen.Define("open", new Action<string>(CommandOpenFile)); // TODO Besser im Define mitgeben wie viele parameter. x für unendlich
			UIConsole.Instance.commands.Add("open", commandOpen);

			UndefinedCommand commandStart = new UndefinedCommand();
			commandStart.Define("start b-d", new Action<string, bool>(CommandStartDay)); // TODO Besser im Define mitgeben wie viele parameter. x für unendlich
			UIConsole.Instance.commands.Add("start", commandStart);

			UndefinedCommand commandSubmit = new UndefinedCommand();
			commandSubmit.Define("submit", new Action<string>(CommandSubmitDay)); // TODO Besser im Define mitgeben wie viele parameter. x für unendlich
			UIConsole.Instance.commands.Add("submit", commandSubmit);
		}

		private void CreateAdventButtons() {
			int colMod = 0;
			int rowMod = 0;
			for (int row = 0; row < 5; row++) {
				colMod = 0;
				for (int column = 0; column < 10; column++) {
					if (column != 0 && column % 2 == 0)
						colMod++;
					int day = row * 5 + column / 2 + 1;

					Button button = new Button();
					button.Click += OnAdventButtonClick;

					var imguri = new Uri(@"pack://application:,,,/images/tree.png", UriKind.Absolute);
					button.Background = new ImageBrush(new BitmapImage(imguri));
					var timezone = TimeZoneInfo.FindSystemTimeZoneById("EST");
					bool happened = DateTimeOffset.Now >= new DateTimeOffset(App.YEAR, 12, day, 0, 0, 0, timezone.GetUtcOffset(DateTimeOffset.Now));
					button.Background.Opacity = happened ? 0.75 : 0.3;
					button.BorderThickness = new Thickness(0.5);
					button.BorderBrush = new SolidColorBrush(Color.FromArgb((byte) (button.Background.Opacity * 255), 23, 179, 39));
					button.Foreground = new SolidColorBrush(Color.FromArgb((byte) (happened ? 255 : button.Background.Opacity * 255), 0, 0, 0));
					button.FontWeight = FontWeights.Bold;
					button.FontSize = 20;
					button.Width = 50;
					button.Height = 50;
					if (happened) {
						this.AOCButtons.Add((true, button));
					} else {
						this.AOCButtons.Add((false, button));
						button.IsEnabled = false;
					}


					if (column % 2 == 0) {
						button.Content = day.ToString("D2") + "A";
					} else {
						button.Content = day.ToString("D2") + "B";
					}
					Grid.SetColumn(button, column + colMod);
					Grid.SetRow(button, row + rowMod);
					adventCalendar.Children.Add(button);
				}
				rowMod++;
			}
		}


		public void OnAdventButtonClick(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			if (button != null) {
				UIConsole.SwitchToExecuteMode();
				foreach (var aocbutton in AOCButtons) {
					aocbutton.button.IsEnabled = false;
				}
				string prog = button.Content.ToString();
				bool isCtrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
				bool isShift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
				bool isAlt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
				Thread thread = new Thread(() => {
					RunAOC(prog, isCtrl, isShift, isAlt);
					Dispatcher.Invoke(() => {
						foreach (var aocbutton in AOCButtons) {
							aocbutton.button.IsEnabled = aocbutton.enabled;
						}
						UIConsole.SwitchToCommandMode();
					});
				});
				thread.Start();
			}
		}

		private void CommandOpenFile(string day) {
			day = ParseDay(day);
			Process.Start("notepad.exe", $"{Environment.CurrentDirectory}/Days/Day{day[..2]}/Day{day}.data");
		}

		private void CommandStartDay(string day, bool IsDebug) {
			day = ParseDay(day);
			Thread.CurrentThread.IsBackground = true;
			Type t = Type.GetType($"AdventOfCoding.Days.Day{day}");
			if (t == null) {
				UIConsole.WriteLine($"Class 'AdventOfCoding.Days.Day{day}' not found!");
			} else {
				DayAbstract aufgabe = (DayAbstract) Activator.CreateInstance(t);
				(string output, Stopwatch time) resultData = (null, null);
				if (Debugger.IsAttached) {
					resultData =
							aufgabe.MainMethod(
								new Reader($"{Environment.CurrentDirectory}/Days/Day{day[..2]}/Day{day}.data"),
								IsDebug
							);
					this.results[day] = resultData.output;
				} else {
					try {
						resultData =
							aufgabe.MainMethod(
								new Reader($"{Environment.CurrentDirectory}/Days/Day{day[..2]}/Day{day}.data"),
								IsDebug
							);
					} catch (Exception exc) {
						UIConsole.WriteLine(exc.StackTrace);
					}
				}
				if (resultData == (null, null)) {
					UIConsole.WriteLine("Not Implemented!");
				} else {
					this.results[day] = resultData.output;
					UIConsole.WriteLine("Output: " + resultData.output);
					UIConsole.WriteLine("Time elapsed: " + (long) (resultData.time.Elapsed.TotalMilliseconds * 1000) + "µs");
				}
			}
		}

		private void CommandSubmitDay(string day) {
			day = ParseDay(day);
			object result = null;
			if (!this.results.TryGetValue(day, out result)) {
				UIConsole.WriteLine($"Missing Result Day{day}");
				return;
			}
			try {
				ProcessStartInfo start = new ProcessStartInfo();
				start.FileName = $"python.exe";
				start.Arguments = string.Format(
						"{0} {1} {2} \"{3}\"",
						$"{AppDomain.CurrentDomain.BaseDirectory}submit.py",
						day[..2],
						day[day.Length - 1].ToString().ToLower(),
						result.ToString()
					);
				start.UseShellExecute = false;
				start.CreateNoWindow = true;
				start.RedirectStandardOutput = true;
				start.RedirectStandardError = true;
				using (Process process = Process.Start(start)) {
					using (StreamReader reader = process.StandardOutput) {
						string stderr = process.StandardError.ReadToEnd();
						string output = reader.ReadToEnd();
						if(!String.IsNullOrEmpty(stderr))
							UIConsole.WriteLine("Errors:\n" + stderr);
						if (!String.IsNullOrEmpty(output))
							UIConsole.WriteLine(output);
					}
				}
			} catch(Exception e) {
				UIConsole.WriteLine(e.Message);
			}
		}


		public void RunAOC(string day, bool isStrg, bool isShift, bool isAlt) {
			day = ParseDay(day);
			if (isStrg) {
				UIConsole.WriteLine($"data Day{day}");
				CommandOpenFile(day);
			} else if (isAlt) {
				UIConsole.WriteLine($"submit Day{day}");
				CommandSubmitDay(day);
			} else {
				UIConsole.WriteLine($"start Day{day}{(isShift ? " -d " : "")}");
				CommandStartDay(day, isShift);
			}
		}

		private string ParseDay(string day) {
			string suffix = "";
			if (day.EndsWith("A")) {
				suffix = day[day.Length - 1].ToString();
				day = day[..(day.Length - 1)];
			} else if (day.EndsWith("B")) {
				suffix = day[day.Length - 1].ToString();
				day = day[..(day.Length - 1)];
			}
			int day_num = int.Parse(day);
			day = day_num.ToString("D2");
			return day + suffix;
		}
	}
}