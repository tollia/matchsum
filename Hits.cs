using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    /// <summary>
    /// Class to implement a List of List of Invoice.
    /// </summary>
    class Hits : List<Hit>
    {
        /// <summary>
        /// Payment that these hits relate to.
        /// </summary>
        private Payment payment;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="payment"></param>
        public Hits(Payment payment)
        {
            this.payment = payment;
        }


        /// <summary>
        /// Get the Invoices object best matching the Payment matched. The longest match is 
        /// cosidered the best. If there are more than one equally long we take the one with
        /// the bigger grand total. If the grand totals match we stay with the solution found 
        /// first.
        /// </summary>
        public Hit BestHit()
        {
            Hit bestHit = null;
            foreach (Hit hit in this)
            {
                if (bestHit != null)
                {

                    // If the length matches look other elements of merit.
                    if (bestHit.Count == hit.Count)
                    {
                        // Pick the solution with the higher grand total.
                        if (bestHit.GrandTotal() < hit.GrandTotal())
                        {
                           bestHit = hit;
                        }
                    }
                    else if (bestHit.Count < hit.Count)
                    {
                        bestHit = hit;
                    }
                }
                else
                {
                    bestHit = hit;
                }
            }
            return bestHit;
        }
    

        /// <summary>
        /// Payment that these Hits where made against.
        /// </summary>
        public Payment Payment
        {
            get { return payment; }
        }


        /// <summary>
        /// Write the invoices in this Hits object one hit per line in comma separated format.
        /// </summary>
        public void Write()
        {
            foreach (Invoices invoices in this)
            {
                invoices.Write();
            }
        }
    }
}
