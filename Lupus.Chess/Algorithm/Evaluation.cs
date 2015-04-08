using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public int Execute(IField field, Move move)
		{
			return Execute(field, move, Side.Black);
		}

		public int Execute(IField field, Move move, Side side)
		{
			var clone = (IField)field.Clone();
			var pieces = move.Side == Side.Black ? clone.BlackPieces : clone.WhitePieces;

			foreach (var piece in pieces.Where(piece => piece.Piece == move.Piece && piece.CurrentPosition == move.From))
			{
				piece.Move(move.To);
			}

			return (BishopPosition.Item1 * BishopPosition.Item2.Execute(clone, side)
				+ CenterControl.Item1 * CenterControl.Item2.Execute(clone, side)
				+ Connectivity.Item1 * Connectivity.Item2.Execute(clone, side)
				+ KnightPosition.Item1 * KnightPosition.Item2.Execute(clone, side)
				+ KingSafety.Item1 * KingSafety.Item2.Execute(clone, side)
				+ Material.Item1 * Material.Item2.Execute(clone, side)
				+ Mobility.Item1 * Mobility.Item2.Execute(clone, side)
				+ PawnStructure.Item1 * PawnStructure.Item2.Execute(clone, side)
				+ QueenPosition.Item1 * QueenPosition.Item2.Execute(clone, side)
				+ RookPosition.Item1 * RookPosition.Item2.Execute(clone, side))
				/ 100;
		}
	}
}
