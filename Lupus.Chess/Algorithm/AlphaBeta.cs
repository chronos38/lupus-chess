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

		public static INode CreateNode(INode previous, Move nextMove)
		{
			var field = (Field) previous.Field.Clone();
			var piece = field.GetPiece(nextMove.From);
			piece.Move(field, nextMove.To);
			var node = new Node(field, nextMove, previous.Depth + 1);
			previous.Add(node);
			return node;
		}
	}
}