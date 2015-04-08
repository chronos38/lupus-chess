using System;

namespace Lupus.Chess.Exception
{
	[Serializable]
	public class ChessException : System.Exception
	{
		public ChessException()
		{
		}

		public ChessException(string message) : base(message)
		{
		}

		public ChessException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}
