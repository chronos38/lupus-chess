using System.Collections.Generic;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IMaterial
	{
		int Execute(IField field);
		int Execute(IField field, Side side);
		int Execute(IEnumerable<IPiece> pieces);
	}
}
