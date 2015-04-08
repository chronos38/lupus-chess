using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Exception
{
	public class ChessMoveException : ChessException
	{
		public ChessMoveException(Move move)
		{
			Move = move;
		}

		public ChessMoveException(Move move, string message) : base(message)
		{
			Move = move;
		}

		public ChessMoveException(Move move, string message, System.Exception innerException)
			: base(message, innerException)
		{
			Move = move;
		}

		public Move Move { get; set; }
	}
}
