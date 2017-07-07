using System;
using System.Linq.Expressions;

namespace Stamper.Patterns
{
    public class BooleanPattern<T> : Pattern<T, bool> where T : class
    {
        public BooleanPattern(Stamp<T> stamp, Expression<Func<T, bool>> property) : base(stamp, property)
        {
        }

        public Stamp<T> Vary()
        {
            return Stamp.Bind(Property, Stamp.Random.Next(0, 2) == 1);
        }
    }
}