﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;
using Lupus.Chess.Piece;
using Lupus.Chess.Tree;

namespace Lupus.Chess.Algorithm
{
	public abstract class AlphaBeta : IAlphaBeta
	{
		public abstract long Execute(INode node, Side plySide, long alpha, long beta, int depth, int ply, int pvIndex);

		/// <summary>
		/// Adds a new node to the tree.
		/// </summary>
		/// <param name="parentNode">Parent where to add the new node.</param>
		/// <param name="nextMove">The move to execute for the new node.</param>
		/// <returns>The new node.</returns>
		public static INode CreateNode(INode parentNode, Move nextMove)
		{
			var field = (Field) parentNode.Field.Clone();
			var piece = field.GetPiece(nextMove.From);
			piece.Move(field, nextMove);
			var node = new Node
			{
				Value = 0,
				Field = field,
				Terminal = field[Side.Both].Count(p => p.Piece == PieceType.King) != 2
			};
			parentNode.Add(node);
			return node;
		}

		/// <summary>
		/// Searches for all possible captures.
		/// </summary>
		/// <param name="node">The node to evaluate.</param>
		/// <param name="plySide">The current ply side.</param>
		/// <param name="alpha">Alpha value.</param>
		/// <param name="beta">Beta value.</param>
		/// <returns>Adjusted alpha value.</returns>
		public static long QuescenceSearch(INode node, Side plySide, long alpha, long beta)
		{
			// TODO: Add delta pruning
			var standPat = Evaluation.Instance.Execute(node.Field);
			if (standPat >= beta) return beta;
			if (alpha < standPat) alpha = standPat;

			var captures = node.AvailableCaptures(plySide);

			foreach (var capture in captures)
			{
				var field = (Field) node.Field.Clone();
				field.ExecuteMove(capture);

				var child = CreateNode(node, capture);
				var value = -QuescenceSearch(child, Move.InvertSide(plySide), -beta, -alpha);

				if (value >= beta) return beta;
				if (value > alpha) alpha = value;
			}

			return alpha;
		}
	}
}