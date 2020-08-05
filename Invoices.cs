using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    /// <summary>
    /// Class to implement List of Invoice and to encapsulate common operations on
    /// a List of Invoice.
    /// </summary>
    class Invoices : List<Invoice>
    {
        /// <summary>
        /// Member to store the grand total of invoices in this list. This may potentially be dangerous
        /// if the list is added to after retrieving the grand total.
        /// </summary>
        private decimal grandTotal = 0;


        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Invoices() : base()
        {
        }


        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="invoices"></param>
        public Invoices(Invoices invoices) : base(invoices)
        {
        }


        /// <summary>
        /// Remove all Invoice objects present in the given invoices list from
        /// this list.
        /// </summary>
        /// <param name="invoices"></param>
        public void Remove(Invoices invoices)
        {
            foreach (Invoice invoice in invoices)
            {
                this.Remove(invoice);
            }
        }


        /// <summary>
        /// Get all invoices in this list that have issue dates older or equal to end date.
        /// </summary>
        /// <param name="endDate">Date up to which to match.</param>
        /// <returns></returns>
        public Invoices GetUpToDate(DateTime endDate)
        {
            Invoices subSet = new Invoices();

            foreach (Invoice invoice in this)
            {
                if (invoice.Date <= endDate)
                {
                    subSet.Add(invoice);
                }
            }

            return subSet;
        }


        /// <summary>
        /// Find the number of days from the given date. This can be used to compare
        /// the merit of equally long Invoices objects when given a payment date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int CummulativeDaysFromDate(DateTime date)
        {
            int days = 0;

            foreach (Invoice invoice in this)
            {
                TimeSpan timeSpan = date - invoice.Date;
                days += Math.Abs(timeSpan.Days);
            }

            return days;
        }


        /// <summary>
        /// Find the grand total of invoices in this list. If this has been previously calulated
        /// we return the stored value to save time.
        /// </summary>
        public decimal GrandTotal()
        {
            decimal grandTotal = 0;
            if (this.grandTotal != 0)
            {
                grandTotal = this.grandTotal;
            }
            else
            {
                foreach (Invoice invoice in this)
                {
                    grandTotal += invoice.Amount;
                }
                this.grandTotal = grandTotal;
            }
            return grandTotal;
        }


        /// <summary>
        /// Write this list in comma separated format.
        /// </summary>
        public void Write()
        {
            string separator = "";
            for (int i = 0; i < this.Count; i++)
            {
                Console.Write(separator + this[i].Amount);
                separator = ",";
            }
            Console.WriteLine();
        }
    }
}
