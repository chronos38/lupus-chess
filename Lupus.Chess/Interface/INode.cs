using System.Collections.Generic;

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
