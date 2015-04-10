using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Exception
{
	public class ChessCastlingException : ChessException
	{
		public Side FromSide { get; set; }

		public ChessCastlingException(Side side)
		{
			FromSide = side;
		}

		public ChessCastlingException(Side side, string message) : base(message)
		{
			FromSide = side;
		}

		public ChessCastlingException(Side side, string message, System.Exception innerException)
			: base(message, innerException)
		{
			FromSide = side;
		}
	}
}
