using System.Collections.Generic;

namespace Lupus.Chess.Interface
{
	public interface INode : ICollection<INode>
	{
		Field Field { get; }
		Move Move { get; }
		IEnumerable<Move> AllowedMoves { get; } 
		long Value { get; set; }
		int Depth { get; set; }
		bool Terminal { get; set; }
	}
}
