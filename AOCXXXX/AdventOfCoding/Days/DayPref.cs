using Lib;
using System;
using System.Diagnostics;

namespace AdventOfCoding.Days {
	public class DayPref : DayAbstract {

		protected override void Runner(Reader reader) {
			string[] lines = reader.ReadAndGetLines();


			for(int i = 0; i < lines.Length; i++) {
				
			}

			this.Result = null;

			/* Output Region */
			if (!IsDebugMode)
				return;
			this.ToPrint.AppendLine("");
		}

	}
}