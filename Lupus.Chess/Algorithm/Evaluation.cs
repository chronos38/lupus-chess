using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class Evaluation : IEvaluation
	{
		public ICollection<Tuple<float, IStrategy>> Strategies { get; set; }

		public int Execute(Field field, Side side)
		{
			if (Strategies == null || Strategies.Count == 0) return 0;
			return
				(int) Math.Round((from pair in Strategies
					let factor = pair.Item1
					let strategy = pair.Item2
					select factor*strategy.Execute(field, side)).Sum());
		}
	}
}
