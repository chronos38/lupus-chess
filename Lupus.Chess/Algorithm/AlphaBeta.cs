using System;
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
		public abstract long Execute(INode node, long alpha, long beta, int depth, IEvaluation evaluation);

		/// <summary>
		/// Adds a new node to the tree.
		/// </summary>
		/// <param name="parentNode">Parent were to add the new node.</param>
		/// <param name="nextMove">The move to execute for the new node.</param>
		/// <returns>The new node.</returns>
		public static INode CreateNode(INode parentNode, Move nextMove)
		{
			var field = (Field) parentNode.Field.Clone();
			var piece = field.GetPiece(nextMove.From);
			piece.Move(field, nextMove);
			var node = new Node(field, nextMove, parentNode.Depth + 1);
			parentNode.Add(node);
			return node;
		}
	}
}