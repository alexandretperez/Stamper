using Stamper.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stamper
{
    public class Stamp<T> where T : class
    {
        private readonly Type _type;
        private readonly List<Action<T>> _bindings;
        private readonly object[] _args;
        private readonly Random _random = new Random();

        public Stamp()
        {
            _type = typeof(T);
            _bindings = new List<Action<T>>();
            _args = new object[] { };
        }

        public Stamp(params object[] args) : this()
        {
            _args = args;
        }

        public Stamp<T> Bind<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            var name = (property.Body as MemberExpression).Member.Name;
            var prop = _type.GetProperty(name);
            return Bind(prop, value);
        }

        private Stamp<T> Bind<TProperty>(PropertyInfo prop, TProperty value)
        {
            if (!prop.CanWrite)
            {
                var args = prop.PropertyType.GetGenericArguments();
                if (args.Length == 0)
                    return this;

                if (typeof(ICollection<>).MakeGenericType(args[0]).IsAssignableFrom(prop.PropertyType))
                    BindList(prop, value);
            }
            else
            {
                _bindings.Add(obj => prop.SetValue(obj, value));
            }
            return this;
        }

        public Stamp<T> Bind<TProperty>(Expression<Func<T, TProperty>> property, Func<StampGenerator<T, TProperty>, TProperty> callback)
        {
            var generator = new StampGenerator<T, TProperty>(this, property);
            return Bind(property, callback(generator));
        }

        private void BindList<TProperty>(PropertyInfo prop, TProperty value)
        {
            var add = prop.PropertyType.GetMethod("Add");
            _bindings.Add(obj =>
            {
                foreach (var item in ((IEnumerable)value))
                    add.Invoke(prop.GetValue(obj), new object[] { item });
            });
        }

        public Random Random => _random;

        public T Perform()
        {
            var constructor = GetConstructor();
            if (constructor == null)
                return null;

            var parameters = _args.Select(p => Expression.Constant(p)).ToArray();
            var obj = Expression.Lambda<Func<T>>(Expression.New(constructor, parameters)).Compile().Invoke();
            foreach (var bind in _bindings)
                bind(obj);

            return obj;
        }

        public Stamp<T> As<TModel>(TModel obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
                Bind(prop, prop.GetValue(obj));

            return this;
        }

        private ConstructorInfo GetConstructor()
        {
            IEnumerable<ConstructorInfo> constructors = _type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            constructors = constructors.Where(p => p.GetParameters().Select(q => q.ParameterType).SequenceEqual(_args.Select(q => q.GetType())));

            return constructors.FirstOrDefault();
        }
    }
}