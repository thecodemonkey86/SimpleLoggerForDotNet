using System;
using System.IO;

namespace SimpleLoggerForDotNet
{
	public class Log
	{
		
		static Log instance = new Log();
		string filepath;
		long limitBytes;
		TextWriter logwriter;
		const string DATETIME = "{datetime}";
		const string TYPE = "{type}";
		const string MESSAGE = "{message}";
		string pattern = "{datetime}\t{type}\t{message}";
		string dateFormat = "yyyy-MM-dd HH:mm:ss";
		
		private Log() {
			
		}
		
		~Log() {
			logwriter.Close();
		}
		
		private void CheckInit() {
			if(filepath == null) {
				throw new InvalidOperationException("Please call method Log.Init first");
			}
		}
		
		public static void Init(string filepath,bool synchronized=false) {
			Init(filepath, synchronized, 0L);
		}
		
		public static void Init(string filepath, long limitBytes) {
			Init(filepath, true, limitBytes);
		}
		
		public static void Init(string filepath,bool synchronized, long limitBytes) {
			Log.instance.limitBytes = limitBytes;
			if(filepath.Contains("."))
				Log.instance.filepath = filepath;
			else
				Log.instance.filepath = filepath + ".log";
			
			if(synchronized)
				Log.instance.logwriter = TextWriter.Synchronized(new StreamWriter(new FileStream(filepath,FileMode.OpenOrCreate|FileMode.Append, FileAccess.Write, FileShare.None)));
			else
				Log.instance.logwriter = new StreamWriter(new FileStream(filepath,FileMode.OpenOrCreate|FileMode.Append, FileAccess.Write, FileShare.None));
		}
		
		public static void SetPattern(string pattern) {
			Log.instance.pattern = pattern;
		}
		
		public static void SetDateFormat(string dateFormat) {
			Log.instance.dateFormat = dateFormat;
		}
		
		private void MaintenanceImpl() {
			if(limitBytes > 0) {
				FileInfo fileinfo = new FileInfo(filepath);
				long oversize = fileinfo.Length - limitBytes;
				if(oversize > 0) {
					using(FileStream fs = new FileStream(filepath,FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)) {
						using(StreamReader reader = new StreamReader(fs)) {
							using(StreamWriter writer = new StreamWriter(fs)) {
								fs.Seek(oversize, SeekOrigin.Begin);
								reader.ReadLine();
								string remainingLog = reader.ReadToEnd();
								fs.SetLength(0);
								writer.Write(remainingLog);
							}
						}
					}
					
				}
				
			} 
		}
		
		public static void Maintenance() {
			Log.instance.MaintenanceImpl();
		}
		
		
		public static void Debug(string message) {
			Log.instance.DebugImpl(message);
		}
		
		public static void Error(string message) {
			Log.instance.ErrorImpl(message);
		}
		
		public static void Error(Exception exception) {
			Log.instance.ErrorImpl(exception);
		}
		
		public static void Info(string message) {
			Log.instance.InfoImpl(message);
		}
		
		public static void Warning(string message) {
			Log.instance.WarningImpl(message);
		}
		
		public static void Critical(string message) {
			Log.instance.CriticalImpl(message);
		}
		
		public static void Critical(Exception exception) {
			Log.instance.CriticalImpl(exception);
		}
		
		private void DebugImpl(string message) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "DEBUG")
                               .Replace(MESSAGE, message)
                              );
		}
		
		private void ErrorImpl(string message) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "ERROR")
                               .Replace(MESSAGE, message)
                              );
		}
		
		private void ErrorImpl(Exception exception) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "ERROR")
                               .Replace(MESSAGE, exception.Message)
                              );
		}
		
		private void InfoImpl(string message) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "INFO")
                               .Replace(MESSAGE, message)
                              );
		}
		
		private void WarningImpl(string message) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "WARNING")
                               .Replace(MESSAGE, message)
                              );
		}
		
		private void CriticalImpl(string message) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "CRITICAL")
                               .Replace(MESSAGE, message)
                              );
		}
		
		private void CriticalImpl(Exception exception) {
			logwriter.WriteLine(pattern
			                    .Replace(DATETIME, DateTime.Now.ToString(dateFormat))
                               .Replace(TYPE, "CRITICAL")
                               .Replace(MESSAGE, exception.ToString())
                              );
		}
		
	}
}