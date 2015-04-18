using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Pawn : AbstractPiece
	{
		public static IEnumerable<Pawn> White
		{
			get
			{
				var result = new Collection<Pawn>();
				var position = new Position
				{
					File = 'A',
					Rank = 2
				};

				for (var i = 0; i < 8; i++)
				{
					result.Add(new Pawn
					{
						Piece = PieceType.Pawn,
						Position = (Position) position.Clone(),
						Side = Side.White
					});

					position.File = (char) (position.File + 1);
				}

				return result;
			}
		}

		public static IEnumerable<Pawn> Black
		{
			get
			{
				var result = new Collection<Pawn>();
				var position = new Position
				{
					File = 'A',
					Rank = 7
				};

				for (var i = 0; i < 8; i++)
				{
					result.Add(new Pawn
					{
						Piece = PieceType.Pawn,
						Position = (Position) position.Clone(),
						Side = Side.Black
					});

					position.File = (char) (position.File + 1);
				}

				return result;
			}
		}

		public bool Promotion
		{
			get
			{
				return Side == Side.White ? Position.Rank == 8 : Position.Rank == 1;
			}
		}

		internal Pawn()
		{
			Piece = PieceType.Pawn;
		}

		internal Pawn(Side side, Position position)
		{
			Piece = PieceType.Pawn;
			Side = side;
			Position = position;
		}

		public override void Move(Field field, Move move)
		{
			if (Side != move.Side || Piece != move.Piece || Position != move.From) throw new ChessMoveException(move);
			var rank = Side == Side.White ? -1 : 1;
			var enPassant = EnPassanExist(field, Side.White, Position);

			if (enPassant != null && enPassant.File == move.To.File && enPassant.Rank == move.To.Rank + rank)
			{
				field.Remove(enPassant);
				Position = move.To;
				History.Instance.Add(move);
			}
			else
			{
				base.Move(field, move);
			}
		}

		public override object Clone()
		{
			return new Pawn
			{
				Piece = Piece,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override ICollection<Position> AllowedPositions(Field field)
		{
			var result = new Collection<Position>();
			var enPassant = EnPassanExist(field, Side, Position);
			if (enPassant != null) result.Add(new Position {File = enPassant.File, Rank = Side == Side.White ? 6 : 3});

			switch (Side)
			{
				case Side.White:
					return WhiteMoves(field, this).Concat(result).ToList();
				case Side.Black:
					return BlackMoves(field, this).Concat(result).ToList();
				default:
					throw new ChessException("Side should be either Side.White or Wide.Black");
			}
		}

		public static Position EnPassanExist(Field field, Side forSide, Position fromPosition)
		{
			var rank = Chess.Move.InvertSide(forSide) == Side.White ? 2 : 7;
			var lastMove = History.Instance.Count > 0 ? History.Instance[History.Instance.Count - 1] : null;
			if (lastMove != null && lastMove.Piece == PieceType.Pawn && lastMove.From.Rank == rank &&
			    Math.Abs(lastMove.From.Rank - lastMove.To.Rank) == 2 && lastMove.To.Rank == fromPosition.Rank &&
			    (lastMove.To.File == fromPosition.File - 1 || lastMove.To.File == fromPosition.File + 1))
			{
				return lastMove.To;
			}

			return null;
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			var result = new List<IPiece>();
			result.AddRange(White);
			result.AddRange(Black);
			return result;
		}

		private static ICollection<Position> WhiteMoves(Field field, IPiece pawn)
		{
			var result = new Collection<Position>();
			var next = Chess.Move.Up(pawn.Position);
			var upperLeft = Chess.Move.UpperLeft(pawn.Position);
			var upperRight = Chess.Move.UpperRight(pawn.Position);

			if (field.IsFree(upperLeft) == Side.Black) result.Add(upperLeft);
			if (field.IsFree(upperRight) == Side.Black) result.Add(upperRight);
			if (field.IsFree(next) == Side.None) result.Add(next);
			if (pawn.Position.Rank != 2) return result;
			var overNext = Chess.Move.Up(next);
			if (field.IsFree(overNext) == Side.None) result.Add(overNext);

			return result;
		}

		private static ICollection<Position> BlackMoves(Field field, IPiece pawn)
		{
			var result = new Collection<Position>();
			var next = Chess.Move.Down(pawn.Position);
			var lowerLeft = Chess.Move.LowerLeft(pawn.Position);
			var lowerRight = Chess.Move.LowerRight(pawn.Position);

			if (field.IsFree(lowerLeft) == Side.White) result.Add(lowerLeft);
			if (field.IsFree(lowerRight) == Side.White) result.Add(lowerRight);
			if (field.IsFree(next) == Side.None) result.Add(next);
			if (pawn.Position.Rank != 7) return result;
			var overNext = Chess.Move.Down(next);
			if (field.IsFree(overNext) == Side.None) result.Add(overNext);

			return result;
		}
	}
}
