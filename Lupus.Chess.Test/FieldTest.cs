using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test
{
	[TestClass]
	public class FieldTest
	{
		readonly Field _field = Field.Start();

		[TestMethod]
		public void Field_CheckSideCount()
		{
			Assert.AreEqual(16, _field.WhitePieces.Count);
			Assert.AreEqual(16, _field.BlackPieces.Count);
		}

		[TestMethod]
		public void Field_CheckWhitePieceCount()
		{
			// Arrange
			var king = 1;
			var queen = 1;
			var bishop = 2;
			var rook = 2;
			var knight = 2;
			var pawn = 8;

			// Act
			foreach (var whitePiece in _field.WhitePieces)
			{
				switch (whitePiece.Piece)
				{
					case PieceType.Bishop:
						bishop -= 1;
						break;
					case PieceType.King:
						king -= 1;
						break;
					case PieceType.Knight:
						knight -= 1;
						break;
					case PieceType.Pawn:
						pawn -= 1;
						break;
					case PieceType.Queen:
						queen -= 1;
						break;
					case PieceType.Rook:
						rook -= 1;
						break;
				}
			}

			// Assert
			Assert.AreEqual(0, pawn);
			Assert.AreEqual(0, queen);
			Assert.AreEqual(0, king);
			Assert.AreEqual(0, rook);
			Assert.AreEqual(0, knight);
			Assert.AreEqual(0, bishop);
		}

		[TestMethod]
		public void Field_CheckBlackPieceCount()
		{
			// Arrange
			var king = 1;
			var queen = 1;
			var bishop = 2;
			var rook = 2;
			var knight = 2;
			var pawn = 8;

			// Act
			foreach (var blackPiece in _field.BlackPieces)
			{
				switch (blackPiece.Piece)
				{
					case PieceType.Bishop:
						bishop -= 1;
						break;
					case PieceType.King:
						king -= 1;
						break;
					case PieceType.Knight:
						knight -= 1;
						break;
					case PieceType.Pawn:
						pawn -= 1;
						break;
					case PieceType.Queen:
						queen -= 1;
						break;
					case PieceType.Rook:
						rook -= 1;
						break;
				}
			}

			// Assert
			Assert.AreEqual(0, pawn);
			Assert.AreEqual(0, queen);
			Assert.AreEqual(0, king);
			Assert.AreEqual(0, rook);
			Assert.AreEqual(0, knight);
			Assert.AreEqual(0, bishop);
		}
	}
}
