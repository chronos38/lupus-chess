using System;
using System.Linq;
using Lupus.Chess.Piece;
using Lupus.Chess.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Tree
{
	[TestClass]
	public class NodeTest
	{
		private Field _empty;

		[TestInitialize]
		public void Initialize()
		{
			_empty = new Field();
		}

		[TestMethod]
		public void Node_AvailableCaptures_EmptyField()
		{
			// Arrange
			var node = new Node(_empty);

			// Act
			var white = node.AvailableCaptures(Side.White);
			var black = node.AvailableCaptures(Side.Black);

			// Assert
			Assert.AreEqual(0, white.Count());
			Assert.AreEqual(0, black.Count());
		}

		[TestMethod]
		public void Node_AvailableCaptures_FewPieces()
		{
			// Arrange
			var node = new Node(_empty);
			node.Field.Add(PieceFactory.Create(PieceType.Queen, Side.White, new Position { File = 'D', Rank = 1 }));
			node.Field.Add(PieceFactory.Create(PieceType.Queen, Side.Black, new Position { File = 'D', Rank = 8 }));

			// Act
			var white = node.AvailableCaptures(Side.White).ToList();
			var black = node.AvailableCaptures(Side.Black).ToList();

			// Assert
			Assert.AreEqual(1, white.Count());
			Assert.AreEqual(new Position {File = 'D', Rank = 8}, white.First().To);
			Assert.AreEqual(1, black.Count());
			Assert.AreEqual(new Position {File = 'D', Rank = 1}, black.First().To);
		}

		[TestMethod]
		public void Node_AvailableCaptures_MorePieces()
		{
			// Arrange
			var node = new Node(_empty);
			node.Field.Add(PieceFactory.Create(PieceType.Queen, Side.White, new Position { File = 'D', Rank = 1 }));
			node.Field.Add(PieceFactory.Create(PieceType.Knight, Side.White, new Position { File = 'E', Rank = 1 }));
			node.Field.Add(PieceFactory.Create(PieceType.Knight, Side.White, new Position { File = 'C', Rank = 1 }));
			node.Field.Add(PieceFactory.Create(PieceType.Queen, Side.Black, new Position { File = 'D', Rank = 8 }));
			node.Field.Add(PieceFactory.Create(PieceType.Knight, Side.Black, new Position { File = 'E', Rank = 8 }));
			node.Field.Add(PieceFactory.Create(PieceType.Knight, Side.Black, new Position { File = 'C', Rank = 8 }));

			// Act
			var white = node.AvailableCaptures(Side.White).ToList();
			var black = node.AvailableCaptures(Side.Black).ToList();

			// Assert
			Assert.AreEqual(1, white.Count());
			Assert.AreEqual(new Position { File = 'D', Rank = 8 }, white.First().To);
			Assert.AreEqual(1, black.Count());
			Assert.AreEqual(new Position { File = 'D', Rank = 1 }, black.First().To);
		}
	}
}
