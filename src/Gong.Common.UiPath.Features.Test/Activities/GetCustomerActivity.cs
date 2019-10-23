using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Gong.Common.UiPath.Features.Test.Activities
{

    public class Customer
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class GetCustomerActivity : CodeActivity<Customer>
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> CustomerId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Name { get; set; }

        [Category("CategoryOne")]
        [RequiredArgument]
        public InArgument<string> SubscriptionKey { get; set; }

        [Category("Input")]
        public InArgument<string> SupplierId { get; set; }

        protected override Customer Execute(CodeActivityContext context)
        {
            return new Customer()
            {
                Id = CustomerId.Get(context),
                Name = Name.Get(context)
            };
        }
    }
}
