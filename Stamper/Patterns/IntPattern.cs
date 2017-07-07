using System;
using System.Linq.Expressions;

namespace Stamper.Patterns
{
    public class IntPattern<T> : Pattern<T, int> where T : class
    {
        public IntPattern(Stamp<T> stamp, Expression<Func<T, int>> property) : base(stamp, property)
        {
        }

        public Stamp<T> Between(int minValue, int maxValue)
        {
            return Stamp.Bind(Property, Stamp.Random.Next(minValue, maxValue + 1));
        }
    }
}