using Lib;
using System.Diagnostics;
using System.Text;

namespace AdventOfCoding.Days {
	public abstract class DayAbstract {

		protected object Result;
		protected StringBuilder ToPrint = new StringBuilder();
		private Stopwatch Stopwatch;
		public bool IsDebugMode { get; private set; }

		public (string output, Stopwatch sw) MainMethod(Reader reader, bool isDebugMode = false) {
			this.IsDebugMode = isDebugMode;
			reader.ReadAndGetLines();
			Stopwatch = new Stopwatch();

			Stopwatch.Start();
			Runner(reader);
			Stopwatch.Stop();

			PrintOutput();

			if (Result == null) {
				return (null, null);
			}

			return (Result.ToString(), Stopwatch);
		}

		protected abstract void Runner(Reader reader);

		public void PrintOutput() {
			if (ToPrint.Length > 0 && IsDebugMode) {
				UIConsole.WriteLine(ToPrint.ToString());
			}
		}

		public bool IfDebugStopTimer() {
			if (IsDebugMode) {
				Stopwatch.Stop();
			}
			return IsDebugMode;
		}

	}
}