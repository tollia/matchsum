using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    /// <summary>
    /// Class to encapsulate all details of an invoice. This class is immutable.
    /// </summary>
    class Invoice : IComparable
    {
        /// <summary>
        /// Invoice serial number.
        /// </summary>
        private int serialNumber;

        /// <summary>
        /// Issue date of invoice.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Amount of invoice.
        /// </summary>
        private decimal amount;


        /// <summary>
        /// Constructuctor
        /// </summary>
        /// <param name="serialNumber">Invoice serial number.</param>
        /// <param name="date">Issue date of invoice.</param>
        /// <param name="amount">Amount of invoice.</param>
        public Invoice(int serialNumber, DateTime date, decimal amount)
        {
            this.serialNumber = serialNumber;
            this.date = date;
            this.amount = amount;
        }


        /// <summary>
        /// Serial number property.
        /// </summary>
        public int SerialNumber
        {
            get
            {
                return serialNumber;
            }
        }

        /// <summary>
        /// Issue Date property.
        /// </summary>
        public DateTime Date
        {
            get 
            {
                return date;
            }
        }

        /// <summary>
        /// Amount of invoice.
        /// </summary>
        public decimal Amount
        {
            get 
            {
                return amount;
            }
        }


        #region IComparable Members

        /// <summary>
        /// Implement IComparable so that comparing two Invoices actually compares their amounts.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is Invoice))
            {
                throw new Exception("Invoice object can only be compared to another Invoice object.");
            }

            return this.amount.CompareTo((obj as Invoice).Amount);
        }

        #endregion
    }
}
