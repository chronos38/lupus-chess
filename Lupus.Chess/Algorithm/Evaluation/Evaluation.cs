using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm.Evaluation
{
	public class Evaluation : IEvaluation
	{
		public static int PawnStrenght { get { return 100; } }

		public ICollection<IStrategy> Strategies { get; set; }

		public int Execute(Field field, Side side)
		{
			if (Strategies == null || Strategies.Count == 0) return 0;
			return (from strategy in Strategies select strategy.Execute(field, side)).Sum();
		}
	}
}
