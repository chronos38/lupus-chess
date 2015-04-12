using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class KingTest
	{
		private Field _emptyField;
		private Field _startField;

		[TestInitialize]
		public void Initialize()
		{
			_emptyField = Field.Create();
			_startField = Field.Start();
		}

		[TestMethod]
		public void King_AllowedPositions_EmptyField()
		{
			// Arrange
			var king = PieceFactory.Create(PieceType.King);
			var positions = new Collection<Position>
			{
				new Position {File = 'E', Rank = 6},
				new Position {File = 'F', Rank = 6},
				new Position {File = 'G', Rank = 6},
				new Position {File = 'E', Rank = 5},
				new Position {File = 'G', Rank = 5},
				new Position {File = 'E', Rank = 4},
				new Position {File = 'F', Rank = 4},
				new Position {File = 'G', Rank = 4}
			};

			// Act
			var allowedPositions = king.AllowedPositions(_emptyField).ToList();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(8, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
		}

		[TestMethod]
		public void King_AllowedPositions_StartField()
		{
			// Arrange
			var king = _startField.WhitePieces.First(p => p.Piece == PieceType.King);

			// Act
			var allowedPositions = king.AllowedPositions(_startField);

			// Assert
			Assert.AreEqual(0, allowedPositions.Count());
		}

		[TestMethod]
		public void King_Castling_AllowedBoth_KingSide()
		{
			// Arrange
			var king = (King) PieceFactory.Create(PieceType.King, Side.White, new Position {File = 'E', Rank = 1});
			_emptyField.WhitePieces.Add(king);
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'A', Rank = 1}));
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'H', Rank = 1}));

			// Act
			var canUseCastling = king.CanUseCastling(_emptyField);
			var rc1 = Move.TryCastling(_emptyField, king, CastlingSide.None);
			var rc2 = Move.TryCastling(_emptyField, king, CastlingSide.Both);
			var rc3 = Move.TryCastling(_emptyField, king, CastlingSide.King);

			// Assert
			Assert.AreEqual(CastlingSide.Both, canUseCastling);
			Assert.IsFalse(rc1);
			Assert.IsFalse(rc2);
			Assert.IsTrue(rc3);
			Assert.AreEqual(new Position {File = 'G', Rank = 1}, king.Position);
		}

		[TestMethod]
		public void King_Castling_AllowedBoth_QueenSide()
		{
			// Arrange
			var king = (King) PieceFactory.Create(PieceType.King, Side.White, new Position {File = 'E', Rank = 1});
			_emptyField.WhitePieces.Add(king);
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'A', Rank = 1}));
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'H', Rank = 1}));

			// Act
			var canUseCastling = king.CanUseCastling(_emptyField);
			var rc1 = Move.TryCastling(_emptyField, king, CastlingSide.None);
			var rc2 = Move.TryCastling(_emptyField, king, CastlingSide.Both);
			var rc3 = Move.TryCastling(_emptyField, king, CastlingSide.Queen);

			// Assert
			Assert.AreEqual(CastlingSide.Both, canUseCastling);
			Assert.IsFalse(rc1);
			Assert.IsFalse(rc2);
			Assert.IsTrue(rc3);
			Assert.AreEqual(new Position { File = 'C', Rank = 1 }, king.Position);
		}

		[TestMethod]
		public void King_Castling_AllowedKing_QueenSide()
		{
			// Arrange
			var king = (King) PieceFactory.Create(PieceType.King, Side.White, new Position {File = 'E', Rank = 1});
			_emptyField.WhitePieces.Add(king);
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'A', Rank = 1}));
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'H', Rank = 1}));
			_emptyField.BlackPieces.Add(PieceFactory.Create(PieceType.Rook, Side.Black, new Position {File = 'D', Rank = 8}));

			// Act
			var canUseCastling = king.CanUseCastling(_emptyField);
			var rc1 = Move.TryCastling(_emptyField, king, CastlingSide.None);
			var rc2 = Move.TryCastling(_emptyField, king, CastlingSide.Both);
			var rc3 = Move.TryCastling(_emptyField, king, CastlingSide.Queen);

			// Assert
			Assert.AreEqual(CastlingSide.King, canUseCastling);
			Assert.IsFalse(rc1);
			Assert.IsFalse(rc2);
			Assert.IsFalse(rc3);
			Assert.AreEqual(new Position { File = 'E', Rank = 1 }, king.Position);
		}

		[TestMethod]
		public void King_Castling_AllowedNone_KingSide()
		{
			// Arrange
			var king = (King) PieceFactory.Create(PieceType.King, Side.White, new Position {File = 'E', Rank = 1});
			_emptyField.WhitePieces.Add(king);
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'A', Rank = 1}));
			_emptyField.WhitePieces.Add(PieceFactory.Create(PieceType.Rook, Side.White, new Position {File = 'H', Rank = 1}));
			_emptyField.BlackPieces.Add(PieceFactory.Create(PieceType.Queen, Side.Black, new Position {File = 'D', Rank = 4}));

			// Act
			var canUseCastling = king.CanUseCastling(_emptyField);
			var rc1 = Move.TryCastling(_emptyField, king, CastlingSide.None);
			var rc2 = Move.TryCastling(_emptyField, king, CastlingSide.Both);
			var rc3 = Move.TryCastling(_emptyField, king, CastlingSide.King);

			// Assert
			Assert.AreEqual(CastlingSide.None, canUseCastling);
			Assert.IsFalse(rc1);
			Assert.IsFalse(rc2);
			Assert.IsFalse(rc3);
			Assert.AreEqual(new Position { File = 'E', Rank = 1 }, king.Position);
		}
	}
}
