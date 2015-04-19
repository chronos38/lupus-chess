using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm
{
	public class AlphaBetaFailHard : AlphaBeta
	{
		public override long Execute(INode node, Side plySide, long alpha, long beta, int depth, History history)
		{
			var array = node.AllowedMoves(plySide).ToArray();

			if (array.Length == 0 || depth <= 0 || node.Terminal)
			{
				var evaluation = QuescenceSearch(node, plySide, alpha, beta, history);
				TranspositionTable.Add(node);
				return evaluation;
			}

			var moves = Sort(node.Field, array);
			node.PastMoves = history;

			foreach (var move in moves)
			{
				var h = (History) history.Clone();
				var child = CreateNode(node, move, h);
				var value = -Execute(child, Move.InvertSide(plySide), -beta, -alpha, depth - 1, h);

				if (value >= beta) return beta; // hard beta cutoff
				if (value <= alpha) continue;
				alpha = value;
				TranspositionTable.Instance[node.Field].Item2.Add(move);
			}

			return alpha;
		}
	}
}