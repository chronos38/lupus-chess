using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess.Tree
{
	internal class Node : INode
	{
		private IList<Move> _moves;

		public Move Move { get; set; }
		public Field Field { get; set; }
		public Side PlySide { get; set; }
		public bool Terminal { get; set; }

		private IList<Move> Moves
		{
			get { return _moves ?? (_moves = AllowedMoves()); }
		}

		public IEnumerator<INode> GetEnumerator()
		{
			return new NodeEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public INode GetChild(int index)
		{
			var nextPlySide = NextPlySide();
			var pieces = nextPlySide == Side.White ? Field.WhitePieces : Field.BlackPieces;
			var opponent = nextPlySide == Side.White ? Field.BlackPieces : Field.WhitePieces;
			var king = (King) (from p in pieces where p.Piece == PieceType.King select p).FirstOrDefault();

			if (king == null) return new Node {Field = Field, Terminal = true};
			if (index < 0) return null;
			if (index >= Moves.Count) return null;
			var move = Moves[index];
			var field = (Field) Field.Clone();

			if (move.CastlingSide != CastlingSide.None)
			{
				Move.Castling(field, king, move.CastlingSide);
				return new Node
				{
					Field = field,
					Move = move,
					PlySide = nextPlySide
				};
			}

			var piece = field.GetPiece(move.From);
			if (piece == null) return null;
			piece.Move(field, move.To);
			// Remove en passant thread
			foreach (var p in opponent.Where(p => p.Piece == PieceType.Pawn)) ((Pawn) p).EnPassantThread = false;

			return new Node
			{
				Field = field,
				Move = move,
				PlySide = nextPlySide
			};
		}

		public INode First()
		{
			return GetChild(0);
		}

		private IList<Move> AllowedMoves()
		{
			return AllowedMoves(NextPlySide(), Field);
		}

		private Side NextPlySide()
		{
			return PlySide == Side.White ? Side.Black : Side.White;
		}

		public static IList<Move> AllowedMoves(Side side, Field field)
		{
			var result = new List<Move>();
			var pieces = side == Side.White ? field.WhitePieces : field.BlackPieces;
			var king = (King) (from piece in pieces where piece.Piece == PieceType.King select piece).FirstOrDefault();

			if (king == null) return new Move[] { };
			AddCastling(result, king.CanUseCastling(field));

			result.AddRange((from piece in pieces
				from position in piece.AllowedPositions(field)
				select new Move
				{
					From = piece.Position,
					To = position,
					Piece = piece.Piece,
					Side = piece.Side
				}));
			return result;
		}

		private static void AddCastling(ICollection<Move> moves,  CastlingSide castling)
		{
			switch (castling)
			{
				case CastlingSide.Both:
					moves.Add(new Move {CastlingSide = CastlingSide.King});
					moves.Add(new Move {CastlingSide = CastlingSide.Queen});
					break;

				case CastlingSide.King:
					moves.Add(new Move {CastlingSide = CastlingSide.King});
					break;

				case CastlingSide.Queen:
					moves.Add(new Move {CastlingSide = CastlingSide.Queen});
					break;
			}
		}

		private class NodeEnumerator : IEnumerator<INode>
		{
			private readonly Node _node;
			private int _index = -1;

			public NodeEnumerator(Node node)
			{
				_node = node;
			}

			public void Dispose()
			{
				// Do nothing
			}

			public bool MoveNext()
			{
				return ++_index < _node.Moves.Count;
			}

			public void Reset()
			{
				_index = 0;
			}

			public INode Current
			{
				get { return _node.GetChild(_index); }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}
		}
	}
}
