using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Tree
{
	public class Node : INode
	{
		public IField Field { get; set; }
		public Side PlySide { get; set; }
		public int Value { get; set; }

		public IEnumerable<Move> AllowedMoves()
		{
			var pieces = PlySide == Side.White ? Field.WhitePieces : Field.BlackPieces;
			return (from piece in pieces
				from position in piece.AllowedPositions()
				select new Move()
				{
					From = piece.Position, To = position, Piece = piece.Piece, Side = PlySide
				}).ToList();
		}
	}
}
