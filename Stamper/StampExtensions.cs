using Stamper.Patterns;
using System;
using System.Linq.Expressions;

namespace Stamper
{
    public static class StampExtensions
    {
        public static IntPattern<T> Bind<T>(this Stamp<T> stamp, Expression<Func<T, int>> property) where T : class
        {
            return new IntPattern<T>(stamp, property);
        }

        public static StringPattern<T> Bind<T>(this Stamp<T> stamp, Expression<Func<T, string>> property) where T : class
        {
            return new StringPattern<T>(stamp, property);
        }

        public static BooleanPattern<T> Bind<T>(this Stamp<T> stamp, Expression<Func<T, bool>> property) where T : class
        {
            return new BooleanPattern<T>(stamp, property);
        }
    }
}