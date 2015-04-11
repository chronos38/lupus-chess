using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class PawnTest
	{
		private Field _emptyField;
		private Field _enemyField;

		[TestInitialize]
		public void Initialize()
		{
			_emptyField = Field.Create();
			_enemyField = Field.Create();
			_enemyField.BlackPieces.Add(PieceFactory.Create(PieceType.Pawn, Side.Black, new Position {File = 'A', Rank = 5}, true));
			_enemyField.BlackPieces.Add(PieceFactory.Create(PieceType.Pawn, Side.Black, new Position {File = 'C', Rank = 5}, true));
			_enemyField.BlackPieces.Add(PieceFactory.Create(PieceType.Pawn, Side.Black, new Position {File = 'E', Rank = 3}, true));
		}

		[TestMethod]
		public void Pawn_AllowedPosition_EmptyField_Rank2()
		{
			// Arrange
			var pawn = (Pawn) PieceFactory.Create(PieceType.Pawn);
			var positions = new Collection<Position>
			{
				new Position {File = 'F', Rank = 3},
				new Position {File = 'F', Rank = 4}
			};

			// Act
			var allowedPositions = pawn.AllowedPositions(_emptyField).ToArray();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(2, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
			Assert.IsFalse(pawn.Promotion);
		}

		[TestMethod]
		public void Pawn_AllowedPosition_EmptyField_Rank4()
		{
			// Arrange
			var pawn = (Pawn) PieceFactory.Create(PieceType.Pawn, Side.White, new Position {File = 'B', Rank = 4}, true);
			var positions = new Collection<Position>
			{
				new Position {File = 'B', Rank = 5}
			};

			// Act
			var allowedPositions = pawn.AllowedPositions(_emptyField).ToArray();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(1, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
			Assert.IsFalse(pawn.Promotion);
		}

		[TestMethod]
		public void Pawn_AllowedPosition_EmptyField_Rank2_WithEnemy()
		{
			// Arrange
			var pawn = (Pawn)PieceFactory.Create(PieceType.Pawn);
			var positions = new Collection<Position>
			{
				new Position {File = 'F', Rank = 3},
				new Position {File = 'F', Rank = 4},
				new Position {File = 'E', Rank = 3}
			};

			// Act
			var allowedPositions = pawn.AllowedPositions(_enemyField).ToArray();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(3, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
			Assert.IsFalse(pawn.Promotion);
		}

		[TestMethod]
		public void Pawn_AllowedPosition_EmptyField_Rank4_WithEnemy()
		{
			// Arrange
			var pawn = (Pawn)PieceFactory.Create(PieceType.Pawn, Side.White, new Position { File = 'B', Rank = 4 }, true);
			var positions = new Collection<Position>
			{
				new Position {File = 'B', Rank = 5},
				new Position {File = 'A', Rank = 5},
				new Position {File = 'C', Rank = 5}
			};

			// Act
			var allowedPositions = pawn.AllowedPositions(_enemyField).ToArray();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(3, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
			Assert.IsFalse(pawn.Promotion);
		}

		[TestMethod]
		public void Pawn_EnPassantPossible()
		{
			// Arrange
			var black = (Pawn) PieceFactory.Create(PieceType.Pawn, Side.Black, new Position {File = 'A', Rank = 7});
			var white = (Pawn) PieceFactory.Create(PieceType.Pawn, Side.White, new Position {File = 'B', Rank = 5});
			_emptyField.BlackPieces.Add(black);
			_emptyField.WhitePieces.Add(white);

			// Act
			var rc1 = black.TryMove(_emptyField, new Position {File = 'A', Rank = 5});
			var rc2 = white.TryMove(_emptyField, new Position {File = 'A', Rank = 6});

			// Assert
			Assert.IsTrue(black.EnPassantThread);
			Assert.IsTrue(rc1);
			Assert.IsTrue(rc2);
			Assert.AreEqual(0, _emptyField.BlackPieces.Count);
		}

		[TestMethod]
		public void Pawn_EnPassantImpossible()
		{
			// Arrange
			var black = (Pawn)PieceFactory.Create(PieceType.Pawn, Side.Black, new Position { File = 'A', Rank = 7 });
			var white = (Pawn)PieceFactory.Create(PieceType.Pawn, Side.White, new Position { File = 'B', Rank = 5 });
			_emptyField.BlackPieces.Add(black);
			_emptyField.WhitePieces.Add(white);

			// Act
			var rc1 = black.TryMove(_emptyField, new Position { File = 'A', Rank = 5 });
			black.EnPassantThread = false;
			var rc2 = white.TryMove(_emptyField, new Position { File = 'A', Rank = 6 });

			// Assert
			Assert.IsTrue(rc1);
			Assert.IsFalse(rc2);
			Assert.AreEqual(1, _emptyField.BlackPieces.Count);
		}
	}
}
