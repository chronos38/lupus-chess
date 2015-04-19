using System;
using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Algorithm.Strategy;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess
{
	public sealed class Evaluation
	{
		private static readonly Lazy<Evaluation> Singleton =
			new Lazy<Evaluation>(() => new Evaluation());

		public static Evaluation Instance { get { return Singleton.Value; } }

		private Evaluation()
		{
			ForSide = Side.White;
			Strategies = new List<IStrategy>
			{
				new Material(),
				new BishopPosition(),
				new KnightPosition(),
				new PawnStructure(),
				new QueenPosition(),
				new RookPosition()
			};
		}

		public static int PawnStrenght { get { return 100; } }

		public Side ForSide { get; set; }
		public IEnumerable<IStrategy> Strategies { get; private set; }

		public int Execute(Field field)
		{
			return Execute(field, ForSide);
		}

		public int Execute(Field field, Side side)
		{
			return (from strategy in Strategies select strategy.Execute(field, ForSide)).Sum();
		}
	}
}
