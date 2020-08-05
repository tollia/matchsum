using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    /// <summary>
    /// A class inheriting from Invoices to add a Payment reference to which this hit relates to.
    /// </summary>
    class Hit : Invoices
    {
        /// <summary>
        /// The payment this hit relates to.
        /// </summary>
        private Payment payment;


        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="hit"></param>
        public Hit(Hit hit) : base(hit)
        {
            this.payment = hit.Payment;
        }


        /// <summary>
        /// Construct a Hit object associated with the given payment. 
        /// </summary>
        /// <param name="payment"></param>
        public Hit(Payment payment)
        {
            this.payment = payment;
        }


        /// <summary>
        /// Payment this hit relates to.
        /// </summary>
        internal Payment Payment
        {
            get { return payment; }
            set { payment = value; }
        }

    }
}
