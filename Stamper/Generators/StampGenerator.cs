using System;
using System.Linq.Expressions;

namespace Stamper.Generators
{
    public class StampGenerator<T, TProperty> where T : class
    {
        public StampGenerator(Stamp<T> stamp, Expression<Func<T, TProperty>> property)
        {
            _stamp = stamp;
            _property = property;
        }

        private Stamp<T> _stamp { get; }

        private Expression<Func<T, TProperty>> _property { get; }

        public TProperty TakeOne(params TProperty[] values)
        {
            var index = _stamp.Random.Next(values.Length);
            return values[index];
        }

        public Random GetRandomizer()
        {
            return _stamp.Random;
        }
    }
}