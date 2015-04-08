using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface IPiece : ICloneable
	{
		Side Side { get; }
		PieceType Piece { get; }
		Position Position { get; }
		void Move(Field field, Position position);
		bool TryMove(Field field, Position position);
		ICollection<Position> AllowedPositions(Field field);
		bool ValidateMove(Field field, Position position);
	}
}
