using System;
using System.Linq.Expressions;

namespace Stamper.Patterns
{
    public abstract class Pattern<T, TProperty> where T : class
    {
        protected Pattern(Stamp<T> stamp, Expression<Func<T, TProperty>> property)
        {
            Stamp = stamp;
            Property = property;
        }

        protected Stamp<T> Stamp { get; }

        protected Expression<Func<T, TProperty>> Property { get; }

        public Stamp<T> With(Func<TProperty> callback)
        {
            return Stamp.Bind(Property, callback());
        }
        public Stamp<T> With(Func<Random, TProperty> callback)
        {
            return Stamp.Bind(Property, callback(Stamp.Random));
        }
    }
}