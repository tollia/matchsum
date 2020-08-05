using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    /// <summary>
    /// Class to encapsulate all details of a payment made.
    /// </summary>
    class Payment
    {
        /// <summary>
        /// Reference of payment acting as a unique ID. This class is immutable.
        /// </summary>
        private string reference;

        /// <summary>
        /// The date the payment was made.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Amount of the payment.
        /// </summary>
        private decimal amount;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">Reference of payment.</param>
        /// <param name="date">Date the payment was made.</param>
        /// <param name="amount">Amount of payment.</param>
        public Payment(string reference, DateTime date, decimal amount)
        {
            this.reference = reference;
            this.date = date;
            this.amount = amount;
        }


        /// <summary>
        /// Reference of payment.
        /// </summary>
        public string Reference
        {
            get { return reference; }
        }

        /// <summary>
        /// Date of payment.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        /// <summary>
        /// Amount of payment.
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
        }
    }
}
