using System;
using System.Globalization;

namespace Lupus.Chess
{
	[Serializable]
	public class Position : ICloneable
	{
		public char File { get; set; }
		public int Rank { get; set; }

		public object Clone()
		{
			return new Position()
			{
				File = File,
				Rank = Rank
			};
		}

		public bool Validate()
		{
			if (File.ToString(CultureInfo.InvariantCulture).IndexOfAny(new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'}) == -1)
				return false;
			return Rank >= 1 && Rank <= 8;
		}

		protected bool Equals(Position other)
		{
			return File == other.File && Rank == other.Rank;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Position)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (File.GetHashCode() * 397) ^ Rank;
			}
		}

		public static bool operator ==(Position lhs, Position rhs)
		{
			return ReferenceEquals(null, lhs) ? ReferenceEquals(null, rhs) : lhs.Equals(rhs);
		}

		public static bool operator !=(Position lhs, Position rhs)
		{
			return ReferenceEquals(null, lhs) ? !ReferenceEquals(null, rhs) : !lhs.Equals(rhs);
		}
	}
}
