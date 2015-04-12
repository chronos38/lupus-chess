using System.Collections.Generic;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Interface
{
	public interface INode : IEnumerable<INode>
	{
		Field Field { get; set; }
		Move Move { get; set; }
		Side PlySide { get; set; }
		INode GetChild(int index);
		INode First();
		void Expand();
	}
}
