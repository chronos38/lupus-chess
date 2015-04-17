using System.Collections.Generic;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm.Evaluation
{
	public abstract class AbstractStrategy : IStrategy
	{
		public int Execute(Field field, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return Compute(field, field.BlackPieces) - Compute(field, field.WhitePieces);
				case Side.White:
					return Compute(field, field.WhitePieces) - Compute(field, field.BlackPieces);
			}

			return 0;
		}

		public abstract int Compute(Field field, ICollection<IPiece> pieces);
	}
}
