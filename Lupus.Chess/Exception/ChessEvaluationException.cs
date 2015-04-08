using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
