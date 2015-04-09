namespace Lupus.Chess.Exception
{
	class ChessEvaluationException : ChessException
	{
		public ChessEvaluationException()
		{
		}

		public ChessEvaluationException(string message) : base(message)
		{
		}

		public ChessEvaluationException(string message, System.Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
