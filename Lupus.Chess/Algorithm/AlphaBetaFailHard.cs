using System.Xml.Schema;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;
using Lupus.Chess.Tree;

namespace Lupus.Chess.Algorithm
{
	public class AlphaBetaFailHard : AlphaBeta
	{
		public override long Execute(INode node, long alpha, long beta, int depth)
		{
			if (depth <= 0 || node.Terminal)
			{
				var evaluation = Evaluation.Execute(node.Field, Move.InvertSide(node.Move.Side));
				TranspositionTable[node.Field] = node;
				node.Value = evaluation;
				return evaluation;
			}

			var moves = node.AllowedMoves;

			foreach (var move in moves)
			{
				var value = -Execute(CreateNode(node, move), -beta, -alpha, depth - 1);

				if (value >= beta) return beta; // hard beta cutoff
				if (value <= alpha) continue;
				alpha = value;
			}

			return alpha;
		}
	}
}