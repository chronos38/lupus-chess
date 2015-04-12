using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class QueenTest
	{
		private readonly Field _emptyField = Field.Create();
		private readonly Field _startField = Field.Start();

		[TestMethod]
		public void Queen_AllowedPositions_EmptyField()
		{
			// Arrange
			var queen = PieceFactory.Create(PieceType.Queen);
			var positions = new Collection<Position>
			{
				new Position {File = 'C', Rank = 4},
				new Position {File = 'B', Rank = 4},
				new Position {File = 'A', Rank = 4},
				new Position {File = 'E', Rank = 4},
				new Position {File = 'F', Rank = 4},
				new Position {File = 'G', Rank = 4},
				new Position {File = 'H', Rank = 4},
				new Position {File = 'D', Rank = 1},
				new Position {File = 'D', Rank = 2},
				new Position {File = 'D', Rank = 3},
				new Position {File = 'D', Rank = 5},
				new Position {File = 'D', Rank = 6},
				new Position {File = 'D', Rank = 7},
				new Position {File = 'D', Rank = 8},
				new Position {File = 'C', Rank = 5},
				new Position {File = 'B', Rank = 6},
				new Position {File = 'A', Rank = 7},
				new Position {File = 'E', Rank = 5},
				new Position {File = 'F', Rank = 6},
				new Position {File = 'G', Rank = 7},
				new Position {File = 'H', Rank = 8},
				new Position {File = 'E', Rank = 3},
				new Position {File = 'F', Rank = 2},
				new Position {File = 'G', Rank = 1},
				new Position {File = 'C', Rank = 3},
				new Position {File = 'B', Rank = 2},
				new Position {File = 'A', Rank = 1}
			};

			// Act
			var allowedPositions = queen.AllowedPositions(_emptyField).ToList();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(27, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
		}

		[TestMethod]
		public void Queen_AllowedPositions_StartField()
		{
			// Arrange
			var queen = _startField.WhitePieces.First(p => p.Piece == PieceType.Queen);

			// Act
			var allowedPositions = queen.AllowedPositions(_startField);

			// Assert
			Assert.AreEqual(0, allowedPositions.Count());
		}
	}
}
