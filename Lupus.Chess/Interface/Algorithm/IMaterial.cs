using System.Collections.Generic;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IMaterial
	{
		int Execute(Field field, Side side);
		int Execute(IEnumerable<IPiece> pieces);
	}
}
