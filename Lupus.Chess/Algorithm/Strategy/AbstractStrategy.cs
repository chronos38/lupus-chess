using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm.Strategy
{
	public abstract class AbstractStrategy : IStrategy
	{
		public int Execute(Field field, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return Compute(field, field[Side.Black]) - Compute(field, field[Side.White]);
				case Side.White:
					return Compute(field, field[Side.White]) - Compute(field, field[Side.Black]);
			}

			return 0;
		}

		public abstract int Compute(Field field, IEnumerable<IPiece> pieces);
	}
}
