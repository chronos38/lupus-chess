using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	public class Field : ICloneable
	{
		public ICollection<IPiece> WhitePieces { get; private set; }
		public ICollection<IPiece> BlackPieces { get; private set; }

		public object Clone()
		{
			return new Field()
			{
				WhitePieces = (from piece in WhitePieces select (IPiece)piece.Clone()).ToList(),
				BlackPieces = (from piece in BlackPieces select (IPiece)piece.Clone()).ToList()
			};
		}

		/// <summary>
		/// Checks if the position is free from any piece.
		/// </summary>
		/// <param name="position">Position to check for.</param>
		/// <returns>Side which is occupying this position or undefined if it is free.</returns>
		public Side IsFree(Position position)
		{
			var piece = (from p in WhitePieces.Concat(BlackPieces) where p.Position == position select p).FirstOrDefault();
			return piece == null ? Side.None : piece.Side;
		}

		/// <summary>
		/// Checks if the position is occupied by a piece from the given side.
		/// </summary>
		/// <param name="position">Position to check for.</param>
		/// <param name="side">The side which pieces shoudl be taken into account.</param>
		/// <returns>true if on peice of given side stands on given position, otherwise false.</returns>
		public bool IsFree(Position position, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return BlackPieces.All(piece => piece.Position != position);
				case Side.White:
					return WhitePieces.All(piece => piece.Position != position);
				default:
					return true;
			}
		}

		public static Field Start()
		{
			// TODO: Implement
			return null;
		}
	}
}
