using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess.Tree
{
	public class Node : Collection<INode>, INode
	{
		public Field Field { get; set; }
		public long Value { get; set; }
		public bool Terminal { get; set; }

		public IEnumerable<Move> AllowedMoves()
		{
			return (from p in Field[Side.Both] from m in p.AllowedMoves(Field) select m);
		}

		public IEnumerable<Move> AllowedMoves(Side side)
		{
			return (from p in Field[side] from m in p.AllowedMoves(Field) select m);
		}

		public IEnumerable<Move> AvailableCaptures(Side fromSide)
		{
			var moves = (from p in Field[fromSide] from m in p.AllowedMoves(Field) select m).ToList();
			return
				Field[Move.InvertSide(fromSide)].Select(p => moves.FirstOrDefault(m => m.To == p.Position))
					.Where(m => m != null)
					.ToList();
		}
	}
}
