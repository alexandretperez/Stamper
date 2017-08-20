using Stamper.Tests.Models;
using Stamper.Generators;
using System;
using Xunit;
using System.Linq;

namespace Stamper.Tests
{
    public class StampTest
    {
        [Fact]
        public void Bind_StaticValues_AllPropertiesAreSet()
        {
            var stamp = new Stamp<Customer>()
                .Bind(p => p.Id, 1)
                .Bind(p => p.Name, "Ace Ventura")
                .Bind(p => p.Email, "customer@one.com")
                .Bind(p => p.BirthDate, new DateTime(1974, 12, 12))
                .Bind(p => p.IsActive, true);

            var customer = stamp.Perform();
            Assert.Equal(1, customer.Id);
            Assert.Equal("Ace Ventura", customer.Name);
            Assert.Equal("customer@one.com", customer.Email);
            Assert.Equal(new DateTime(1974, 12, 12), customer.BirthDate);
            Assert.True(customer.IsActive);
        }

        [Fact]
        public void As_AnonymousModel_AllPropertiesAreSet()
        {
            var stamp = new Stamp<Customer>()
                    .As(new
                    {
                        Id = 2,
                        Name = "Gandalf",
                        Email = "gandalf@one.com",
                        BirthDate = new DateTime(1900, 1, 1),
                        IsActive = true
                    });

            var customer = stamp.Perform();
            Assert.Equal(2, customer.Id);
            Assert.Equal("Gandalf", customer.Name);
            Assert.Equal("gandalf@one.com", customer.Email);
            Assert.Equal(new DateTime(1900, 1, 1), customer.BirthDate);
            Assert.True(customer.IsActive);
        }


        [Fact]
        public void Generators_TakeOne_Test()
        {
            var stamp = new Stamp<Customer>()
                .Bind(p => p.Id, gen => gen.Between(1, 5))
                .Bind(p => p.Name, gen => gen.TakeOne("Alice", "Brandon", "Camile", "Daniel"));

            var customer = stamp.Perform();
            Assert.InRange(customer.Id, 1, 5);
            Assert.True(new[] { "Alice", "Brandon", "Camile", "Daniel" }.Contains(customer.Name));
        }
    }
}