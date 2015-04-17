using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess.Tree
{
	public class Node : INode
	{
		private readonly ICollection<INode> _nodes = new Collection<INode>();

		public Node(Field field, Move move, int depth)
		{
			Field = field;
			Move = move;
			Depth = depth;
			Value = 0;
			Terminal = field.WhitePieces.Concat(field.BlackPieces).Count(p => p.Piece == PieceType.King) != 2;
			AllowedMoves = ComputeAllowedMoves(Move.InvertSide(move.Side), field);
		}

		public IEnumerator<INode> GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(INode item)
		{
			_nodes.Add(item);
		}

		public void Clear()
		{
			_nodes.Clear();
		}

		public bool Contains(INode item)
		{
			return _nodes.Contains(item);
		}

		public void CopyTo(INode[] array, int arrayIndex)
		{
			_nodes.CopyTo(array, arrayIndex);
		}

		public bool Remove(INode item)
		{
			return _nodes.Remove(item);
		}

		public int Count { get { return _nodes.Count; } }
		public bool IsReadOnly { get { return false; } }
		public Field Field { get; set; }
		public Move Move { get; set; }
		public IEnumerable<Move> AllowedMoves { get; set; }
		public long Value { get; set; }
		public int Depth { get; set; }
		public bool Terminal { get; set; }

		public static IEnumerable<Move> ComputeAllowedMoves(Side side, Field field)
		{
			return (from piece in field[side] from move in piece.AllowedMoves(field) select move).ToList();
		}
	}
}
