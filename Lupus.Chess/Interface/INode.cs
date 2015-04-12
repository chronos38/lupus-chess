using System.Collections.Generic;

namespace Lupus.Chess.Interface
{
	public interface INode : IEnumerable<INode>
	{
		Field Field { get; set; }
		Move Move { get; set; }
		Side PlySide { get; set; }
		bool Terminal { get; set; }
		INode GetChild(int index);
		INode First();
	}
}
