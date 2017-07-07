using System;
using System.Linq.Expressions;

namespace Stamper.Patterns
{
    public class StringPattern<T> : Pattern<T, string> where T : class
    {
        public StringPattern(Stamp<T> stamp, Expression<Func<T, string>> property) : base(stamp, property)
        {
        }

        public Stamp<T> TakeOne(params string[] args)
        {
            var index = Stamp.Random.Next(0, args.Length);
            return Stamp.Bind(Property, args[index]);
        }
    }
}