using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class BishopTest
	{
		private readonly Field _emptyField = Field.Create();
		private readonly Field _startField = Field.Start();

		[TestMethod]
		public void Bishop_AllowedPositions_EmptyField()
		{
			// Arrange
			var bishop = PieceFactory.Create(PieceType.Bishop);
			var positions = new Collection<Position>
			{
				new Position {File = 'C', Rank = 4},
				new Position {File = 'B', Rank = 3},
				new Position {File = 'A', Rank = 2},
				new Position {File = 'C', Rank = 6},
				new Position {File = 'B', Rank = 7},
				new Position {File = 'A', Rank = 8},
				new Position {File = 'E', Rank = 6},
				new Position {File = 'F', Rank = 7},
				new Position {File = 'G', Rank = 8},
				new Position {File = 'E', Rank = 4},
				new Position {File = 'F', Rank = 3},
				new Position {File = 'G', Rank = 2},
				new Position {File = 'H', Rank = 1}
			};

			// Act
			var allowedPositions = bishop.AllowedPositions(_emptyField).ToArray();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(13, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
		}

		[TestMethod]
		public void Bishop_AllowedPositions_StartField()
		{
			// Arrange
			var bishop = _startField.WhitePieces.First(p => p.Piece == PieceType.Bishop && p.Position.File == 'C');

			// Act
			var allowedPositions = bishop.AllowedPositions(_startField);

			// Assert
			Assert.AreEqual(0, allowedPositions.Count());
		}
	}
}
