﻿using NLog;

namespace CareDomain
{

	public static class LoggerMiddlewareb
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		public static void LogDebug(string message)
		{
			logger.Debug(message);
		}

		public static void LogError(string message)
		{
			logger.Error(message);
		}

		public static void LogInfo(string message)
		{
			logger.Info(message);
		}

		public static void LogWarn(string message)
		{
			logger.Warn(message);
		}
		public static void LogTrace(string message)
		{
			logger.Trace(message);
		}


	}
}