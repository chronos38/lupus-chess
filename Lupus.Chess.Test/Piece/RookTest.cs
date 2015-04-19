using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class RookTest
	{
		private readonly Field _emptyField = new Field();
		private readonly Field _startField = Field.Start();

		[TestMethod]
		public void Rook_AllowedPositions_EmptyField()
		{
			// Arrange
			var rook = PieceFactory.Create(PieceType.Rook);
			var positions = new Collection<Position>
			{
				new Position {File = 'C', Rank = 5},
				new Position {File = 'B', Rank = 5},
				new Position {File = 'A', Rank = 5},
				new Position {File = 'E', Rank = 5},
				new Position {File = 'F', Rank = 5},
				new Position {File = 'G', Rank = 5},
				new Position {File = 'H', Rank = 5},
				new Position {File = 'D', Rank = 1},
				new Position {File = 'D', Rank = 2},
				new Position {File = 'D', Rank = 3},
				new Position {File = 'D', Rank = 4},
				new Position {File = 'D', Rank = 6},
				new Position {File = 'D', Rank = 7},
				new Position {File = 'D', Rank = 8}
			};

			// Act
			var allowedPositions = rook.AllowedPositions(_emptyField).ToList();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(14, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
		}

		[TestMethod]
		public void Rook_AllowedPositions_StartField()
		{
			// Arrange
			var rook = _startField[Side.White].First(p => p.Piece == PieceType.Rook && p.Position.File == 'A');

			// Act
			var allowedPositions = rook.AllowedPositions(_startField);

			// Assert
			Assert.AreEqual(0, allowedPositions.Count());
		}
	}
}
