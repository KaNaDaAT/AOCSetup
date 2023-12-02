using System;
using System.ComponentModel;
using System.IO;

namespace Lib {
	public class Reader {

		public string _filePath;
		public string FilePath
		{
			get { return _filePath; }
			set { _filePath = value; }
		}

		private string Content = null;
		private string[] ContentLines = null;

		public Reader(string filePath, bool readOnInit = true)
		{
			this.FilePath = filePath;
			if (readOnInit)
			{
				ReadAndGetLines();
			}
		}


		public string ReadAll()
		{
			if (Content == null)
				Content = File.ReadAllText(_filePath);
			return Content;
		}
		public string ReadAllWithoutR()
		{
			return ReadAll().Replace("\r", "");
		}

		public string GetContent()
		{
			return Content;
		}

		public string[] ReadAndGetLines()
		{
			if (ContentLines == null)
			{
				Content = File.ReadAllText(_filePath);
				ContentLines = Content.Replace("\r", "").Split('\n');
			}

			return ContentLines;
		}

		public T[] ReadAndGetLines<T>(bool removeEmpty = true)
		{
			if (ContentLines == null)
			{
				Content = File.ReadAllText(_filePath);
				ContentLines = Content.Replace("\r", "").Split('\n');
			}

			T[] contentAsT = new T[ContentLines.Length];
			for (int i = 0; i < ContentLines.Length; i++)
			{
				if (ContentLines[i] == "")
					continue;
				try
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
					if (converter == null)
					{
						return null;
					}
					contentAsT[i] = (T)converter.ConvertFromString(ContentLines[i]);
				}
				catch (NotSupportedException)
				{
					return null;
				}
			}
			return contentAsT;

		}

		public void Clear()
		{
			Content = null;
			ContentLines = null;
		}

		public static string CurrentDir(string append = "")
		{
			append = (
				append.StartsWith("/") || append.StartsWith("\\") || append == "" ?
				append : Path.DirectorySeparatorChar + append
			);
			return Directory.GetCurrentDirectory() + append;
		}
	}
}
