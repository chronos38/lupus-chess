using System;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IEvaluation
	{
		Tuple<int, IBishopPosition> BishopPosition { get; set; }
		Tuple<int, ICenterControl> CenterControl { get; set; }
		Tuple<int, IConnectivity> Connectivity { get; set; }
		Tuple<int, IKnightPosition> KnightPosition { get; set; }
		Tuple<int, IKingSafety> KingSafety { get; set; }
		Tuple<int, IMaterial> Material { get; set; }
		Tuple<int, IMobility> Mobility { get; set; }
		Tuple<int, IPawnStructure> PawnStructure { get; set; }
		Tuple<int, IQueenPosition> QueenPosition { get; set; }
		Tuple<int, IRookPosition> RookPosition { get; set; }
		int Execute(IField field, Move move);
		int Execute(IField field, Move move, Side side);
	}
}
