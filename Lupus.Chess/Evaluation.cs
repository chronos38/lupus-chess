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
			Strategies = new List<IStrategy>()
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
		public ICollection<IStrategy> Strategies { get; private set; }

		public int Execute(Field field)
		{
			return Execute(field, ForSide);
		}

		public int Execute(Field field, Side side)
		{
			if (Strategies == null) throw new NullReferenceException("Evaluation.Strategies is null");
			if (Strategies.Count == 0) throw new InvalidOperationException("Evaluation.Strategies is empty.");
			return (from strategy in Strategies select strategy.Execute(field, ForSide)).Sum();
		}
	}
}
