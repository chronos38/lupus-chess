namespace Lupus.Chess.Exception
{
	public class ChessPositionException : ChessException
	{
		public ChessPositionException(Position position)
		{
			Position = position;
		}

		public ChessPositionException(Position position, string message)
			: base(message)
		{
			Position = position;
		}

		public ChessPositionException(Position position, string message, System.Exception innerException)
			: base(message, innerException)
		{
			Position = position;
		}

		public Position Position { get; set; }
	}
}
