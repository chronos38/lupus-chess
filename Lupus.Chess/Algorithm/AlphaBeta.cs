using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;
using Lupus.Chess.Piece;
using Lupus.Chess.Tree;

namespace Lupus.Chess.Algorithm
{
	public abstract class AlphaBeta : IAlphaBeta
	{
		public abstract long Execute(INode node, Side plySide, long alpha, long beta, int depth, History history);

		/// <summary>
		/// Adds a new node to the tree.
		/// </summary>
		/// <param name="parentNode">Parent where to add the new node.</param>
		/// <param name="nextMove">The move to execute for the new node.</param>
		/// <param name="history">The previous made moves. Used for injection.</param>
		/// <returns>The new node.</returns>
		public static INode CreateNode(INode parentNode, Move nextMove, History history)
		{
			INode node;
			var field = (Field) parentNode.Field.Clone();

			switch (nextMove.CastlingSide)
			{
				case CastlingSide.King:
				case CastlingSide.Queen:
					var king = (King) field[nextMove.Side].First(p => p.Piece == PieceType.King);
					Move.Castling(field, king, nextMove.CastlingSide);
					break;
				default:
					var piece = field.GetPiece(nextMove.From);
					piece.Move(field, nextMove);
					break;
			}

			lock (TranspositionTable.Instance)
			{
				if (TranspositionTable.Instance.ContainsKey(field))
				{
					node = TranspositionTable.Instance[field].Item1;
				}
				else
				{
					node = new Node(field);
					TranspositionTable.Add(node);
				}
			}

			parentNode.Add(node);
			return node;
		}

		/// <summary>
		/// Searches for all possible captures so the AI does not lose through a silly move.
		/// </summary>
		/// <param name="node">The node to evaluate.</param>
		/// <param name="plySide">The current ply side.</param>
		/// <param name="alpha">Alpha value.</param>
		/// <param name="beta">Beta value.</param>
		/// <param name="history">The previous made moves. Used for injection.</param>
		/// <returns>Adjusted alpha value.</returns>
		public static long QuescenceSearch(INode node, Side plySide, long alpha, long beta, History history)
		{
			var evaluation = node.Value ?? Evaluation.Instance.Execute(node.Field);
			if (node.Value == null) node.Value = evaluation;
			if (evaluation >= beta) return beta;
			if (evaluation > alpha) alpha = evaluation;

			node.PastMoves = history;

			foreach (var capture in node.AvailableCaptures(plySide))
			{
				var h = (History) history.Clone();
				var child = CreateNode(node, capture, h);
				var value = -QuescenceSearch(child, Move.InvertSide(plySide), -beta, -alpha, h);

				if (value >= beta) return beta;
				if (value > alpha) alpha = value;
			}

			return alpha;
		}

		public static IEnumerable<Move> Sort(Field field, IEnumerable<Move> moves)
		{
			var scores = new List<Tuple<long, Move>>();

			foreach (var move in moves)
			{
				Move.Do(field, move);
				scores.Add(new Tuple<long, Move>(Evaluation.Instance.Execute(field, move.Side), move));
				Move.Undo(field, move);
			}

			var result = new List<Move>();
			var indexSet = new HashSet<int>();

			for (var i = 0; i < Math.Min(5, scores.Count); i++)
			{
				long max = int.MinValue;
				var maxLocation = 0;

				for (var j = 0; j < scores.Count; j++)
				{
					if (scores[j].Item1 <= max) continue;
					max = scores[j].Item1;
					maxLocation = j;
				}

				result.Add(scores[maxLocation].Item2);
				scores[maxLocation] = new Tuple<long, Move>(int.MinValue, scores[maxLocation].Item2);
				indexSet.Add(maxLocation);
			}

			result.AddRange(scores.Where((t, i) => !indexSet.Contains(i)).Select(t => t.Item2));
			return result;
		}
	}
}