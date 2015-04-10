using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class KingTest
	{
		private readonly Field _emptyField = Field.Create();
		private readonly Field _startField = Field.Start();

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
			var allowedPositions = king.AllowedPositions(_emptyField).ToArray();
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
	}
}
