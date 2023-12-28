#if UNITY_EDITOR
#define DEBUG
#define ASSERT
#endif
using System.Diagnostics;
using UnityEngine;

namespace _Framework
{
	public static class Common
	{
		//-----------------------------------
		//--------------------- Log , warning, 
		
		[Conditional("DEBUG")]
		public static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		[Conditional("DEBUG")]
		public static void Log(string format, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format(format, args));
		}

		[Conditional("DEBUG")]
		public static void LogWarning(object message, Object context)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}

		[Conditional("DEBUG")]
		public static void LogWarning(Object context, string format, params object[] args)
		{
			UnityEngine.Debug.LogWarning(string.Format(format, args), context);
		}



		[Conditional("DEBUG")]
		public static void Warning(bool condition, object message)
		{
			if ( ! condition) UnityEngine.Debug.LogWarning(message);
		}

		[Conditional("DEBUG")]
		public static void Warning(bool condition, object message, Object context)
		{
			if ( ! condition) UnityEngine.Debug.LogWarning(message, context);
		}

		[Conditional("DEBUG")]
		public static void Warning(bool condition, Object context, string format, params object[] args)
		{
			if ( ! condition) UnityEngine.Debug.LogWarning(string.Format(format, args), context);
		}


		//---------------------------------------------
		//------------- Assert ------------------------

		/// Throw an exception if condition = false
		[Conditional("ASSERT")]
		public static void Assert(bool condition)
		{
			if (! condition) throw new UnityException();
		}

		/// Throw an exception if condition = false, show message on console's log
		[Conditional("ASSERT")]
		public static void Assert(bool condition, string message)
		{
			if (! condition) throw new UnityException(message);
		}

		/// Throw an exception if condition = false, show message on console's log
		[Conditional("ASSERT")]
		public static void Assert(bool condition, string format, params object[] args)
		{
			if (! condition) throw new UnityException(string.Format(format, args));
		}
	}
}