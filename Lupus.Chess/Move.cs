using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	[Serializable]
	public class Move
	{
		public Position From { get; set; }
		public Position To { get; set; }
		public PieceType Piece { get; set; }
		public Side Side { get; set; }

		protected bool Equals(Move other)
		{
			return Equals(From, other.From) && Equals(To, other.To) && Piece == other.Piece && Side == other.Side;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Move)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (From != null ? From.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)Piece;
				hashCode = (hashCode * 397) ^ (int)Side;
				return hashCode;
			}
		}

		public static bool operator ==(Move lhs, Move rhs)
		{
			return ReferenceEquals(null, lhs) ? ReferenceEquals(null, rhs) : lhs.Equals(rhs);
		}

		public static bool operator !=(Move lhs, Move rhs)
		{
			return ReferenceEquals(null, lhs) ? !ReferenceEquals(null, rhs) : !lhs.Equals(rhs);
		}
	}
}
