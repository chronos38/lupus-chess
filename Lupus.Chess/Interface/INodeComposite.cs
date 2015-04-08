using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface INodeComposite : INode, ICollection<INode>
	{
		INode this[int index] { get; }
		INode GetChild(int index);
	}
}
