using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Interface.Algorithm;
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
		public void Tree_StartField_FailSoft_Depth1()
		{
			// Arrange
			var field = _startField;
			var root = new Node(field);
			var alphaBeta = new AlphaBetaFailSoft();
			ITreeSearch treeSearch = new TreeSearch(root, alphaBeta);

			// Act
			var move = treeSearch.Execute(1);
			Task.WaitAll(treeSearch.Tasks.ToArray());

			// Assert?
			Assert.AreNotEqual(0, TranspositionTable.Instance.Count);
		}

		[TestMethod]
		public void Tree_StartField_FailSoft_Depth2()
		{
			// Arrange
			var field = _startField;
			var root = new Node(field);
			var alphaBeta = new AlphaBetaFailSoft();
			ITreeSearch treeSearch = new TreeSearch(root, alphaBeta);

			// Act
			var move = treeSearch.Execute(2);
			Task.WaitAll(treeSearch.Tasks.ToArray());

			// Assert?
			Assert.AreNotEqual(0, TranspositionTable.Instance.Count);
		}

		[TestMethod]
		public void Tree_StartField_FailSoft_Depth3()
		{
			// Arrange
			var field = _startField;
			var root = new Node(field);
			var alphaBeta = new AlphaBetaFailSoft();
			ITreeSearch treeSearch = new TreeSearch(root, alphaBeta);

			// Act
			var move = treeSearch.Execute(3);
			Task.WaitAll(treeSearch.Tasks.ToArray());

			// Assert?
			Assert.AreNotEqual(0, TranspositionTable.Instance.Count);
		}

		[TestMethod]
		public void Tree_StartField_FailSoft_Depth4()
		{
			// Arrange
			var field = _startField;
			var root = new Node(field);
			var alphaBeta = new AlphaBetaFailSoft();
			ITreeSearch treeSearch = new TreeSearch(root, alphaBeta);

			// Act
			var move = treeSearch.Execute(4, 20000);
			Task.WaitAll(treeSearch.Tasks.ToArray());

			// Assert?
			Assert.AreNotEqual(0, TranspositionTable.Instance.Count);
		}
	}
}
