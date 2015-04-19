using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm
{
	public class AlphaBetaFailSoft : AlphaBeta
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

			long bestvalue = int.MinValue;
			var moves = Sort(node.Field, array);
			node.PastMoves = history;

			foreach (var move in moves)
			{
				var h = (History) history.Clone();
				var child = CreateNode(node, move, h);
				var value = -Execute(child, Move.InvertSide(plySide), -beta, -alpha, depth - 1, h);

				if (value >= beta) return value; // soft beta cutoff
				if (value <= bestvalue) continue;
				bestvalue = value;
				TranspositionTable.Instance[child.Field].Item2.Add(move);
				if (value <= alpha) continue;
				alpha = value;
			}

			return bestvalue;
		}
	}
}