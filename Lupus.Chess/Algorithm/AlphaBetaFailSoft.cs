using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;
using Lupus.Chess.Tree;

namespace Lupus.Chess.Algorithm
{
	public class AlphaBetaFailSoft : AlphaBeta
	{
		public override long Execute(INode node, Side plySide, long alpha, long beta, int depth, int ply, int pvIndex)
		{
			if (depth <= 0 || node.Terminal)
			{
				var evaluation = QuescenceSearch(node, plySide, alpha, beta);
				TranspositionTable.Instance[node.Field] = node;
				node.Value = evaluation;
				return evaluation;
			}

			long bestvalue = int.MinValue;
			TpvTable.Instance[pvIndex] = null;
			var pvNextIndex = pvIndex + TpvTable.N - ply;

			foreach (var move in node.AllowedMoves(plySide))
			{
				var child = CreateNode(node, move);
				var value = -Execute(child, plySide, -beta, -alpha, depth - 1, ply + 1, pvNextIndex);

				if (value >= beta) return value; // soft beta cutoff
				if (value <= bestvalue) continue;
				bestvalue = value;
				if (value <= alpha) continue;
				alpha = value;
				TpvTable.Instance[pvIndex] = move;
				TpvTable.Instance.MoveCopy(pvIndex + 1, pvNextIndex, TpvTable.N - ply - 1);
			}

			return bestvalue;
		}
	}
}