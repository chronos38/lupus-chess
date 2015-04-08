using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IKingSafety
	{
		int Execute(Field field, Side side);
		int Execute(IEnumerable<IPiece> pieces);
	}
}
