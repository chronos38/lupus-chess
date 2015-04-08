using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface
{
	public interface IField : ICloneable
	{
		ICollection<IPiece> WhitePieces { get; }
		ICollection<IPiece> BlackPieces { get; }
	}
}
