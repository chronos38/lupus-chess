using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test
{
	[TestClass]
	public class FieldTest
	{
		private Field _field;

		[TestInitialize]
		public void Initialize()
		{
			_field = Field.Start();
		}

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

		[TestMethod]
		public void Field_CheckStartValues_White()
		{
			// Arrange
			var set = new HashSet<char>();
			var pieces = _field.WhitePieces;
			var pieceCount = new Dictionary<PieceType, int>
			{
				{PieceType.King, 0},
				{PieceType.Queen, 0},
				{PieceType.Bishop, 0},
				{PieceType.Knight, 0},
				{PieceType.Rook, 0},
				{PieceType.Pawn, 0}
			};
			var positions = new Dictionary<PieceType, ICollection<Position>>
			{
				{PieceType.King, new List<Position> {new Position {File = 'E', Rank = 1}}},
				{PieceType.Queen, new List<Position> {new Position {File = 'D', Rank = 1}}},
				{PieceType.Bishop, new List<Position> {new Position {File = 'C', Rank = 1}, new Position {File = 'F', Rank = 1}}},
				{PieceType.Knight, new List<Position> {new Position {File = 'B', Rank = 1}, new Position {File = 'G', Rank = 1}}},
				{PieceType.Rook, new List<Position> {new Position {File = 'A', Rank = 1}, new Position {File = 'H', Rank = 1}}}
			};

			// Act
			foreach (var piece in pieces)
			{
				if (positions.ContainsKey(piece.Piece))
				{
					Assert.IsTrue(positions[piece.Piece].Contains(piece.Position));
					positions[piece.Piece].Remove(piece.Position);
					pieceCount[piece.Piece] += 1;
				}
				if (piece.Piece != PieceType.Pawn) continue;
				Assert.AreEqual(2, piece.Position.Rank);
				Assert.AreNotEqual(-1,
					piece.Position.File.ToString(CultureInfo.InvariantCulture)
						.IndexOfAny(new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'}));
				pieceCount[piece.Piece] += 1;
				set.Add(piece.Position.File);
			}

			// Assert
			Assert.AreEqual(8, set.Count);
			Assert.AreEqual(0, positions[PieceType.King].Count);
			Assert.AreEqual(0, positions[PieceType.Queen].Count);
			Assert.AreEqual(0, positions[PieceType.Bishop].Count);
			Assert.AreEqual(0, positions[PieceType.Knight].Count);
			Assert.AreEqual(0, positions[PieceType.Rook].Count);
			Assert.AreEqual(1, pieceCount[PieceType.King]);
			Assert.AreEqual(1, pieceCount[PieceType.Queen]);
			Assert.AreEqual(2, pieceCount[PieceType.Bishop]);
			Assert.AreEqual(2, pieceCount[PieceType.Knight]);
			Assert.AreEqual(2, pieceCount[PieceType.Rook]);
			Assert.AreEqual(8, pieceCount[PieceType.Pawn]);
		}

		[TestMethod]
		public void Field_CheckStartValues_Black()
		{
			// Arrange
			var set = new HashSet<char>();
			var pieces = _field.BlackPieces;
			var pieceCount = new Dictionary<PieceType, int>
			{
				{PieceType.King, 0},
				{PieceType.Queen, 0},
				{PieceType.Bishop, 0},
				{PieceType.Knight, 0},
				{PieceType.Rook, 0},
				{PieceType.Pawn, 0}
			};
			var positions = new Dictionary<PieceType, ICollection<Position>>
			{
				{PieceType.King, new List<Position> {new Position {File = 'E', Rank = 8}}},
				{PieceType.Queen, new List<Position> {new Position {File = 'D', Rank = 8}}},
				{PieceType.Bishop, new List<Position> {new Position {File = 'C', Rank = 8}, new Position {File = 'F', Rank = 8}}},
				{PieceType.Knight, new List<Position> {new Position {File = 'B', Rank = 8}, new Position {File = 'G', Rank = 8}}},
				{PieceType.Rook, new List<Position> {new Position {File = 'A', Rank = 8}, new Position {File = 'H', Rank = 8}}}
			};

			// Act
			foreach (var piece in pieces)
			{
				if (positions.ContainsKey(piece.Piece))
				{
					Assert.IsTrue(positions[piece.Piece].Contains(piece.Position));
					positions[piece.Piece].Remove(piece.Position);
					pieceCount[piece.Piece] += 1;
				}
				if (piece.Piece != PieceType.Pawn) continue;
				Assert.AreEqual(7, piece.Position.Rank);
				Assert.AreNotEqual(-1,
					piece.Position.File.ToString(CultureInfo.InvariantCulture)
						.IndexOfAny(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' }));
				pieceCount[piece.Piece] += 1;
				set.Add(piece.Position.File);
			}

			// Assert
			Assert.AreEqual(8, set.Count);
			Assert.AreEqual(0, positions[PieceType.King].Count);
			Assert.AreEqual(0, positions[PieceType.Queen].Count);
			Assert.AreEqual(0, positions[PieceType.Bishop].Count);
			Assert.AreEqual(0, positions[PieceType.Knight].Count);
			Assert.AreEqual(0, positions[PieceType.Rook].Count);
			Assert.AreEqual(1, pieceCount[PieceType.King]);
			Assert.AreEqual(1, pieceCount[PieceType.Queen]);
			Assert.AreEqual(2, pieceCount[PieceType.Bishop]);
			Assert.AreEqual(2, pieceCount[PieceType.Knight]);
			Assert.AreEqual(2, pieceCount[PieceType.Rook]);
			Assert.AreEqual(8, pieceCount[PieceType.Pawn]);
		}
	}
}
