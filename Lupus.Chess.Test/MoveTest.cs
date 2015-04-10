using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test
{
	[TestClass]
	public class MoveTest
	{
		[TestMethod]
		public void Move_Up()
		{
			// Arrange
			var position = new Position {File = 'A', Rank = 1};

			// Act
			var result = Move.Up(position);

			// Assert
			Assert.AreEqual('A', result.File);
			Assert.AreEqual(2, result.Rank);
		}

		[TestMethod]
		public void Move_Down()
		{
			// Arrange
			var position = new Position { File = 'A', Rank = 2 };

			// Act
			var result = Move.Down(position);

			// Assert
			Assert.AreEqual('A', result.File);
			Assert.AreEqual(1, result.Rank);
		}
		
		[TestMethod]
		public void Move_Rigt()
		{
			// Arrange
			var position = new Position { File = 'A', Rank = 1 };

			// Act
			var result = Move.Right(position);

			// Assert
			Assert.AreEqual('B', result.File);
			Assert.AreEqual(1, result.Rank);
		}

		[TestMethod]
		public void Move_Left()
		{
			// Arrange
			var position = new Position { File = 'B', Rank = 1 };

			// Act
			var result = Move.Left(position);

			// Assert
			Assert.AreEqual('A', result.File);
			Assert.AreEqual(1, result.Rank);
		}

		[TestMethod]
		public void Move_UpperLeft()
		{
			// Arrange
			var position = new Position { File = 'B', Rank = 2 };

			// Act
			var result = Move.UpperLeft(position);

			// Assert
			Assert.AreEqual('A', result.File);
			Assert.AreEqual(3, result.Rank);
		}

		[TestMethod]
		public void Move_UpperRight()
		{
			// Arrange
			var position = new Position { File = 'B', Rank = 2 };

			// Act
			var result = Move.UpperRight(position);

			// Assert
			Assert.AreEqual('C', result.File);
			Assert.AreEqual(3, result.Rank);
		}

		[TestMethod]
		public void Move_LowerLeft()
		{
			// Arrange
			var position = new Position { File = 'B', Rank = 2 };

			// Act
			var result = Move.LowerLeft(position);

			// Assert
			Assert.AreEqual('A', result.File);
			Assert.AreEqual(1, result.Rank);
		}

		[TestMethod]
		public void Move_LowerRight()
		{
			// Arrange
			var position = new Position { File = 'B', Rank = 2 };

			// Act
			var result = Move.LowerRight(position);

			// Assert
			Assert.AreEqual('C', result.File);
			Assert.AreEqual(1, result.Rank);
		}
	}
}
