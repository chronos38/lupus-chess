using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Piece;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Piece
{
	[TestClass]
	public class KnightTest
	{
		private readonly Field _emptyField = new Field();
		private readonly Field _startField = Field.Start();

		[TestMethod]
		public void Knight_AllowedPositions_EmptyField()
		{
			// Arrange
			var knight = PieceFactory.Create(PieceType.Knight);
			var positions = new Collection<Position>
			{
				new Position {File = 'B', Rank = 3},
				new Position {File = 'B', Rank = 5},
				new Position {File = 'C', Rank = 2},
				new Position {File = 'C', Rank = 6},
				new Position {File = 'F', Rank = 3},
				new Position {File = 'F', Rank = 5},
				new Position {File = 'E', Rank = 2},
				new Position {File = 'E', Rank = 6}
			};
			
			// Act
			var allowedPositions = knight.AllowedPositions(_emptyField).ToList();
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
		public void Knight_AllowedPositions_StartField()
		{
			// Arrange
			var knight = _startField[Side.White].First(p => p.Piece == PieceType.Knight && p.Position.File == 'B');
			var positions = new Collection<Position>
			{
				new Position {File = 'A', Rank = 3},
				new Position {File = 'C', Rank = 3}
			};

			// Act
			var allowedPositions = knight.AllowedPositions(_startField).ToList();
			var intersection = positions.ToList();
			foreach (var allowedPosition in allowedPositions)
			{
				intersection.Remove(allowedPosition);
			}

			// Assert
			Assert.AreEqual(2, allowedPositions.Count());
			Assert.IsTrue(allowedPositions.All(positions.Contains));
			Assert.AreEqual(0, intersection.Count);
		}
	}
}
