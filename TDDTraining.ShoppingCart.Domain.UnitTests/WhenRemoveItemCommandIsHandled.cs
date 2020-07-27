using System;
using TDDTraining.ShoppingCart.Domain.CommandHandlers;
using TDDTraining.ShoppingCart.Domain.Commands;
using TDDTraining.ShoppingCart.Domain.Core;
using Xunit;

namespace TDDTraining.ShoppingCart.Domain.UnitTests
{
    public class WhenRemoveItemCommandIsHandled : WhenHandlingCartCommand<RemoveItemCommand, RemoveItemCommandHandler>
    {
        [Fact]
        public void ProductIsNotPresentInCart()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            GivenProductAlreadyExistsInCart(productId, customerId);

            var command = new RemoveItemCommand(customerId, productId);
            var cart = WhenCommandIsHandled<OkResult<Cart>>(command).Body;
            
            Assert.DoesNotContain(cart.Itens, x => x.ProductId == productId);
        }

        [Fact]
        public void IfCartDoesNotExistsCommandDoesNotFailAndNewCartIsCreatedForCustomer()
        {
            var command = new RemoveItemCommand(Guid.NewGuid(), Guid.NewGuid());
            
            var cart = WhenCommandIsHandled<OkResult<Cart>>(command).Body;
            
            AssertNewCartWasCreatedToTheCustomer(cart, command);
        }

        private static void AssertNewCartWasCreatedToTheCustomer(Cart cart, RemoveItemCommand command)
        {
            Assert.NotNull(cart);
            Assert.Equal(command.CustomerId, cart.CustomerId);
        }

        protected override RemoveItemCommandHandler CreateCommandHandler()
        {
            return new RemoveItemCommandHandler(Repository);
        }
    }
}