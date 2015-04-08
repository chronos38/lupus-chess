using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class Evaluation : IEvaluation
	{
		public Tuple<int, IBishopPosition> BishopPosition { get; set; }
		public Tuple<int, ICenterControl> CenterControl { get; set; }
		public Tuple<int, IConnectivity> Connectivity { get; set; }
		public Tuple<int, IKnightPosition> KnightPosition { get; set; }
		public Tuple<int, IKingSafety> KingSafety { get; set; }
		public Tuple<int, IMaterial> Material { get; set; }
		public Tuple<int, IMobility> Mobility { get; set; }
		public Tuple<int, IPawnStructure> PawnStructure { get; set; }
		public Tuple<int, IQueenPosition> QueenPosition { get; set; }
		public Tuple<int, IRookPosition> RookPosition { get; set; }

		public int Execute(Field field, Move move)
		{
			return (BishopPosition.Item1 * BishopPosition.Item2.Execute(field, move.Side)
				+ CenterControl.Item1 * CenterControl.Item2.Execute(field, move.Side)
				+ Connectivity.Item1 * Connectivity.Item2.Execute(field, move.Side)
				+ KnightPosition.Item1 * KnightPosition.Item2.Execute(field, move.Side)
				+ KingSafety.Item1 * KingSafety.Item2.Execute(field, move.Side)
				+ Material.Item1 * Material.Item2.Execute(field, move.Side)
				+ Mobility.Item1 * Mobility.Item2.Execute(field, move.Side)
				+ PawnStructure.Item1 * PawnStructure.Item2.Execute(field, move.Side)
				+ QueenPosition.Item1 * QueenPosition.Item2.Execute(field, move.Side)
				+ RookPosition.Item1 * RookPosition.Item2.Execute(field, move.Side))
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
