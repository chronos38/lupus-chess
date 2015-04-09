using System.Collections.Generic;

namespace Lupus.Chess.Interface
{
	public interface INodeComposite : INode, ICollection<INode>
	{
		INode this[int index] { get; }
		INode GetChild(int index);
	}
}
