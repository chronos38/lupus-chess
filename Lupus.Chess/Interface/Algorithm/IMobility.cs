using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IMobility
	{
		int Execute(IField field);
		int Execute(IField field, Side side);
		int Execute(IEnumerable<IPiece> pieces);
	}
}
