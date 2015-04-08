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
		PieceType Piece { get; }
		Position Position { get; }
		void Move(Position position);
		bool TryMove(Position position);
		ICollection<Position> AllowedPositions();
	}
}
