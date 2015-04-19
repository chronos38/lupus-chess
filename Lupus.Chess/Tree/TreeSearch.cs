using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	public class TreeSearch : ITreeSearch
	{
		private ICollection<Task> _tasks = new Collection<Task>();
		public INode Root { get; set; }
		public IAlphaBeta AlphaBeta { get; set; }

		public TreeSearch(INode root, IAlphaBeta alphaBeta)
		{
			if (root == null) throw new ArgumentNullException("root");
			if (alphaBeta == null) throw new ArgumentNullException("alphaBeta");
			AlphaBeta = alphaBeta;
			Root = root;
		}

		public IEnumerable<Move> Execute(int depth)
		{
			return Execute(depth, 0);
		}

		public IEnumerable<Move> Execute(int depth, int timeout)
		{
			return Execute(depth, timeout, null);
		}

		public IEnumerable<Move> Execute(int depth, int timeout, INode root)
		{
			var node = root ?? Root;
			TranspositionTable.Add(node);
			IterativeDeepening(node, AlphaBeta, depth, timeout);
			return TranspositionTable.Instance[node.Field].Item2;
		}

		public ICollection<Task> Tasks
		{
			get { return _tasks; }
			internal set { _tasks = value; }
		}

		/// <summary>
		/// Finds the node that corresponds to the given field.
		/// </summary>
		/// <param name="root">Root node.</param>
		/// <param name="currentField">Field one ply ahead of root.</param>
		/// <returns>The new root node.</returns>
		public INode FindCorrespondingNode(INode root, Field currentField)
		{
			if (currentField == null) return root;
			lock (TranspositionTable.Instance)
			{
				return TranspositionTable.Instance.ContainsKey(currentField)
					? TranspositionTable.Instance[currentField].Item1
					: root;
			}
		}

		public void IterativeDeepening(INode root, IAlphaBeta alphaBeta, int depth, int timeout)
		{
			var start = DateTime.UtcNow;
			var executionQueue = new Collection<INode> {root};
			var nextQueue = new Collection<INode>();

			while (depth > 0)
			{
				var side = depth%2 == 0 ? Evaluation.Instance.ForSide : Move.InvertSide(Evaluation.Instance.ForSide);

				foreach (var node in executionQueue)
				{
					var node1 = node;
					var queue = nextQueue;

					if (timeout > 0 && (DateTime.UtcNow - start).Milliseconds > timeout)
					{
						foreach (var n in executionQueue.TakeWhile(n => n != node1))
						{
							n.Clear();
						}
						break;
					}

					Tasks.Add(Task.Factory.StartNew(() =>
					{
						alphaBeta.Execute(node1, side, int.MinValue, int.MaxValue, 1, (History) History.Instance.Clone());

						foreach (var n in node1)
						{
							queue.Add(n);
						}

						return node1;
					}));
				}

				Task.WaitAll(Tasks.ToArray());
				Tasks.Clear();
				depth -= 1;
				executionQueue = nextQueue;
				nextQueue = new Collection<INode>();

				if (timeout > 0 && (DateTime.UtcNow - start).Milliseconds > timeout) break;
			}
		}
	}
}
