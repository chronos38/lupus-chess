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
		}
	}
}
