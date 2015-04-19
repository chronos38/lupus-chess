using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Knight : AbstractPiece
	{
		public static IEnumerable<Knight> White
		{
			get
			{
				return new Collection<Knight>
				{
					new Knight
					{
						Piece = PieceType.Knight,
						Side = Side.White,
						Position = new Position
						{
							File = 'B',
							Rank = 1
						}
					},
					new Knight
					{
						Piece = PieceType.Knight,
						Side = Side.White,
						Position = new Position
						{
							File = 'G',
							Rank = 1
						}
					}
				};
			}
		}

		public static IEnumerable<Knight> Black
		{
			get
			{
				return new Collection<Knight>
				{
					new Knight
					{
						Piece = PieceType.Knight,
						Side = Side.Black,
						Position = new Position
						{
							File = 'B',
							Rank = 8
						}
					},
					new Knight
					{
						Piece = PieceType.Knight,
						Side = Side.Black,
						Position = new Position
						{
							File = 'G',
							Rank = 8
						}
					}
				};
			}
		}

		internal Knight()
		{
			Piece = PieceType.Knight;
		}

		internal Knight(Side side, Position position)
		{
			Piece = PieceType.Knight;
			Side = side;
			Position = position;
		}

		public override object Clone()
		{
			return new Knight
			{
				Piece = PieceType.Knight,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new Collection<Position>();
			var upperLeft = Chess.Move.UpperLeft(Position);
			var upperRight = Chess.Move.UpperRight(Position);
			var lowerLeft = Chess.Move.LowerLeft(Position);
			var lowerRight = Chess.Move.LowerRight(Position);

			if (upperLeft.Validate())
			{
				var up = Chess.Move.Up(upperLeft);
				var left = Chess.Move.Left(upperLeft);
				if (up.Validate() && field.IsFree(up, Side)) result.Add(up);
				if (left.Validate() && field.IsFree(left, Side)) result.Add(left);
			}

			Position right;
			if (upperRight.Validate())
			{
				var up = Chess.Move.Up(upperRight);
				right = Chess.Move.Right(upperRight);
				if (up.Validate() && field.IsFree(up, Side)) result.Add(up);
				if (right.Validate() && field.IsFree(right, Side)) result.Add(right);
			}

			Position down;
			if (lowerLeft.Validate())
			{
				down = Chess.Move.Down(lowerLeft);
				var left = Chess.Move.Left(lowerLeft);
				if (down.Validate() && field.IsFree(down, Side)) result.Add(down);
				if (left.Validate() && field.IsFree(left, Side)) result.Add(left);
			}

			if (!lowerRight.Validate()) return result;
			down = Chess.Move.Down(lowerRight);
			right = Chess.Move.Right(lowerRight);
			if (down.Validate() && field.IsFree(down, Side)) result.Add(down);
			if (right.Validate() && field.IsFree(right, Side)) result.Add(right);

			return result;
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			var result = new List<IPiece>();
			result.AddRange(White);
			result.AddRange(Black);
			return result;
		}
	}
}
