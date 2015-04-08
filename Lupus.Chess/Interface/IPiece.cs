using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface IPiece
	{
		Side Side { get; }
		Piece Piece { get; }
		Position CurrentPosition { get; }
		void Move(Position position);
		bool TryMove(Position position);
		ICollection<Position> AllowedPositions();
	}
}
