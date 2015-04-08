using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Tree
{
	public class NodeComposite : INodeComposite
	{
		private readonly Collection<INode> _nodes = new Collection<INode>(); 
		public int Count { get { return _nodes.Count; } }
		public bool IsReadOnly { get { return false; } }
		public IField Field { get; set; }
		public Side PlySide { get; set; }
		public int Value { get; set; }

		public INode this[int index]
		{
			get { return _nodes[index]; }
		}

		public INode GetChild(int index)
		{
			return _nodes[index];
		}

		public IEnumerable<Move> AllowedMoves()
		{
			var result = new List<Move>();

			foreach (var node in _nodes)
			{
				result.AddRange(node.AllowedMoves());
			}

			return result;
		}

		public IEnumerator<INode> GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}

		public void Add(INode item)
		{
			_nodes.Add(item);
		}

		public void Clear()
		{
			_nodes.Clear();
		}

		public bool Contains(INode item)
		{
			return _nodes.Contains(item);
		}

		public void CopyTo(INode[] array, int arrayIndex)
		{
			_nodes.CopyTo(array, arrayIndex);
		}

		public bool Remove(INode item)
		{
			return _nodes.Remove(item);
		}
	}
}
