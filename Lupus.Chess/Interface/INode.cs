using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface INode
	{
		Field Field { get; set; }
		Side PlySide { get; set; }
		int Value { get; set; }
		IEnumerable<Move> AllowedMoves();
	}
}
