using System;
using Lupus.Chess.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lupus.Chess.Test.Tree
{
	[TestClass]
	public class TreeTest
	{
		private Field _startField;

		[TestInitialize]
		public void Initialize()
		{
			_startField = Field.Start();
		}

		[TestMethod]
		public void Tree_StartField_AiWhite_DefaultDepth()
		{
			// Arrange
			var treeSearch = TreeFactory.Create(Side.White);

			// Act
			var move = treeSearch.Execute(_startField, 1).Result;

			// Assert
 			Assert.AreNotEqual(null, move);
		}
	}
}
