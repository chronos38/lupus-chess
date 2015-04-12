using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
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
						Moved = false,
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
						Moved = false,
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

		public bool EnPassantThread { get; set; }

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

		internal Pawn(Side side, Position position, bool moved)
		{
			Moved = moved;
			Piece = PieceType.Bishop;
			Side = side;
			Position = position;
		}

		public override void Move(Field field, Position position)
		{
			var pawn = FindEnPassant(field);

			if (pawn != null)
			{
				switch (Side)
				{
					case Side.White:
						if (pawn.Position.Rank == position.Rank - 1 && pawn.Position.File == position.File) field.Remove(pawn.Position);
						break;
					case Side.Black:
						if (pawn.Position.Rank == position.Rank + 1 && pawn.Position.File == position.File) field.Remove(pawn.Position);
						break;
				}
			}

			EnPassantThread = Math.Abs(position.Rank - Position.Rank) == 2;
			base.Move(field, position);
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
			switch (Side)
			{
				case Side.White:
					return WhiteMoves(field, this);
				case Side.Black:
					return BlackMoves(field, this);
				default:
					return new Position[] { };
			}
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			var result = new List<IPiece>();
			result.AddRange(White);
			result.AddRange(Black);
			return result;
		}

		public Pawn FindEnPassant(Field field)
		{
			switch (Side)
			{
				case Side.White:
					return FindEnPassantWhite(field);
				case Side.Black:
					return FindEnPassantBlack(field);
				default:
					return null;
			}
		}

		private Pawn FindEnPassantWhite(Field field)
		{
			// Check if pawn is on correct rank
			if (Position.Rank != 5) return null;
			// Check if a pawn is nearby
			var left = field.GetPiece(Chess.Move.Left(Position));
			var right = field.GetPiece(Chess.Move.Right(Position));
			// Return left pawn if it is valid
			if (left != null && left.Piece == PieceType.Pawn && left.Side == Side.Black && ((Pawn) left).EnPassantThread)
				return (Pawn) left;
			// Return right pawn if it is valid
			if (right != null && right.Piece == PieceType.Pawn && right.Side == Side.Black && ((Pawn) right).EnPassantThread)
				return (Pawn) right;
			// No en passant possible
			return null;
		}

		private Pawn FindEnPassantBlack(Field field)
		{
			// Check if pawn is on correct rank
			if (Position.Rank != 4) return null;
			// Check if a pawn is nearby
			var left = field.GetPiece(Chess.Move.Left(Position));
			var right = field.GetPiece(Chess.Move.Right(Position));
			// Return left pawn if it is valid
			if (left != null && left.Piece == PieceType.Pawn && left.Side == Side.White && ((Pawn) left).EnPassantThread)
				return (Pawn) left;
			// Return right pawn if it is valid
			if (right != null && right.Piece == PieceType.Pawn && right.Side == Side.White && ((Pawn) right).EnPassantThread)
				return (Pawn) right;
			// No en passant possible
			return null;
		}

		private static ICollection<Position> WhiteMoves(Field field, Pawn pawn)
		{
			var result = new Collection<Position>();
			var next = Chess.Move.Up(pawn.Position);
			var upperLeft = Chess.Move.UpperLeft(pawn.Position);
			var upperRight = Chess.Move.UpperRight(pawn.Position);

			if (field.IsFree(upperLeft) == Side.Black) result.Add(upperLeft);
			if (field.IsFree(upperRight) == Side.Black) result.Add(upperRight);
			if (field.IsFree(next) == Side.None) result.Add(next);
			if (pawn.Moved) return result;
			var overNext = Chess.Move.Up(next);
			if (field.IsFree(overNext) == Side.None) result.Add(overNext);

			// En passant
			var enPassant = pawn.FindEnPassant(field);
			if (enPassant != null) result.Add(new Position {File = enPassant.Position.File, Rank = 6});

			return result;
		}

		private static ICollection<Position> BlackMoves(Field field, Pawn pawn)
		{
			var result = new Collection<Position>();
			var next = Chess.Move.Down(pawn.Position);
			var lowerLeft = Chess.Move.LowerLeft(pawn.Position);
			var lowerRight = Chess.Move.LowerRight(pawn.Position);

			if (field.IsFree(lowerLeft) == Side.White) result.Add(lowerLeft);
			if (field.IsFree(lowerRight) == Side.White) result.Add(lowerRight);
			if (field.IsFree(next) == Side.None) result.Add(next);
			if (pawn.Moved) return result;
			var overNext = Chess.Move.Down(next);
			if (field.IsFree(overNext) == Side.None) result.Add(overNext);

			// En passant
			var enPassant = pawn.FindEnPassant(field);
			if (enPassant != null) result.Add(new Position { File = enPassant.Position.File, Rank = 3 });

			return result;
		}
	}
}
