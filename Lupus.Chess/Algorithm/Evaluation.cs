using System;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class Evaluation : IEvaluation
	{
		public Tuple<int, IStrategy> BishopPosition { get; set; }
		public Tuple<int, IStrategy> CenterControl { get; set; }
		public Tuple<int, IStrategy> Connectivity { get; set; }
		public Tuple<int, IStrategy> KnightPosition { get; set; }
		public Tuple<int, IStrategy> KingSafety { get; set; }
		public Tuple<int, IStrategy> Material { get; set; }
		public Tuple<int, IStrategy> Mobility { get; set; }
		public Tuple<int, IStrategy> PawnStructure { get; set; }
		public Tuple<int, IStrategy> QueenPosition { get; set; }
		public Tuple<int, IStrategy> RookPosition { get; set; }

		public int Execute(Field field, Side side)
		{
			return (BishopPosition.Item1 * BishopPosition.Item2.Execute(field, side)
				+ CenterControl.Item1 * CenterControl.Item2.Execute(field, side)
				+ Connectivity.Item1 * Connectivity.Item2.Execute(field, side)
				+ KnightPosition.Item1 * KnightPosition.Item2.Execute(field, side)
				+ KingSafety.Item1 * KingSafety.Item2.Execute(field, side)
				+ Material.Item1 * Material.Item2.Execute(field, side)
				+ Mobility.Item1 * Mobility.Item2.Execute(field, side)
				+ PawnStructure.Item1 * PawnStructure.Item2.Execute(field, side)
				+ QueenPosition.Item1 * QueenPosition.Item2.Execute(field, side)
				+ RookPosition.Item1 * RookPosition.Item2.Execute(field, side))
				/ (BishopPosition.Item1
				+ CenterControl.Item1
				+ Connectivity.Item1
				+ KnightPosition.Item1
				+ KingSafety.Item1
				+ Material.Item1
				+ Mobility.Item1
				+ PawnStructure.Item1
				+ QueenPosition.Item1
				+ RookPosition.Item1);
		}
	}
}
