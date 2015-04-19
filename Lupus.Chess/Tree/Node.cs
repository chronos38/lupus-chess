using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess.Tree
{
	public class Node : List<INode>, INode
	{
		private History _pastMoves;
		public Field Field { get; set; }
		public long? Value { get; set; }
		public bool Terminal { get; set; }

		public History PastMoves
		{
			get { return _pastMoves; }
			set
			{
				_pastMoves = value;
				foreach (var piece in Field) piece.PastMoves = value;
			}
		}

		public Node(Field field)
		{
			Field = field;
			Value = null;
			Terminal = field[Side.Both].Count(p => p.Piece == PieceType.King) != 2;
		}

		public IEnumerable<Move> AllowedMoves(Side side)
		{
			return (from p in Field[side] from m in p.AllowedMoves(Field) select m);
		}

		public IEnumerable<Move> AvailableCaptures(Side fromSide)
		{
			var moves = (from p in Field[fromSide] from m in p.AllowedMoves(Field) select m);
			return
				Field[Move.InvertSide(fromSide)].Select(p => moves.FirstOrDefault(m => m.To == p.Position))
					.Where(m => m != null);
		}

		public void Sort(Order order)
		{
			switch (order)
			{
				case Order.Ascending:
					Sort((lhs, rhs) =>
					{
						if (lhs.Value == rhs.Value) return 0;
						if (lhs.Value < rhs.Value) return -1;
						return 1;
					});
					break;
				case Order.Descending:
					Sort((lhs, rhs) =>
					{
						if (lhs.Value == rhs.Value) return 0;
						if (lhs.Value < rhs.Value) return 1;
						return -1;
					});
					break;
			}
		}
	}
}
