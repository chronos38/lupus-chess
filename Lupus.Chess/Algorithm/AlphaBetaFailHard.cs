using System.Xml.Schema;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;
using Lupus.Chess.Tree;

namespace Lupus.Chess.Algorithm
{
	public class AlphaBetaFailHard : AlphaBeta
	{
		public override long Execute(INode node, long alpha, long beta, int depth, IEvaluation evaluation)
		{
			if (depth == 0 || node.Terminal) return evaluation.Execute(node.Field, Move.InvertSide(node.Move.Side));

			var moves = node.AllowedMoves;

			foreach (var move in moves)
			{
				var value = -Execute(CreateNode(node, move), -beta, -alpha, depth - 1, evaluation);

				if (value >= beta) return beta; // hard beta cutoff
				if (value <= alpha) continue;
				alpha = value;
			}

			return alpha;
		}
	}
}