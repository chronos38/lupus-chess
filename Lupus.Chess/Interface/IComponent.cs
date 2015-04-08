using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface IComponent : ICloneable
	{
		IDictionary<Side, ICollection<Piece>> Pieces { get; }
		Side PlySide { get; set; }
		int Value { get; set; }
		ICollection<Move> AllowedMoves();
	}
}
