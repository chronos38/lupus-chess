using System;
using System.Collections.Generic;
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

		public static Field Default()
		{
			// TODO: Implement
			return null;
		}
	}
}
