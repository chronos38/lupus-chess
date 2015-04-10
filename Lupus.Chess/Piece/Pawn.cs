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

		public bool Moved { get; protected set; }

		public override bool TryMove(Field field, Position position)
		{
			Moved = true;
			return base.TryMove(field, position);
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

		public override IEnumerable<Position> AllowedPositions(Field field)
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

		private static IEnumerable<Position> WhiteMoves(Field field, Pawn pawn)
		{
			var result = new Collection<Position>();
			var next = Chess.Move.Up(pawn.Position);
			var upperLeft = Chess.Move.UpperLeft(pawn.Position);
			var upperRight = Chess.Move.UpperLeft(pawn.Position);

			if (field.IsFree(upperLeft) == Side.Black) result.Add(upperLeft);
			if (field.IsFree(upperRight) == Side.Black) result.Add(upperRight);
			if (field.IsFree(next) == Side.None) result.Add(next);
			if (pawn.Moved) return result;
			var overNext = Chess.Move.Up(next);
			if (field.IsFree(overNext) == Side.None) result.Add(overNext);

			return result;
		}

		private static IEnumerable<Position> BlackMoves(Field field, Pawn pawn)
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

			return result;
		}
	}
}
