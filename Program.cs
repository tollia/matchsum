using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{

    /// <summary>
    /// Simple set of methods to implement the recursive version of the method outlined in the 
    /// Italian academic paper. This version is 20% faster than the first, losing 10% from version
    /// 2 as we have now wrapped the invoice values into an Invoice object so that serial numbers
    /// and issue dates are available in the match.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Generate simple data set.
        /// </summary>
        /// <param name="invoices">Out parameter for Invoices generated.</param>
        /// <param name="payments">Out parameter for payments.</param>
        private static void GenerateSimpleData(out Invoices invoices, out List<Payment> payments)
        {
            invoices = new Invoices();
            payments = new List<Payment>();

            int serial = 1;
            Invoice invoice;
            for (int i = 0; i < 1; i++)
            {
                invoice = new Invoice(serial++, DateTime.Now, 1); invoices.Add(invoice);
                invoice = new Invoice(serial++, DateTime.Now, 3); invoices.Add(invoice);
                invoice = new Invoice(serial++, DateTime.Now, 5); invoices.Add(invoice);
                invoice = new Invoice(serial++, DateTime.Now, 6); invoices.Add(invoice);
            }

            payments.Add(new Payment("00001", DateTime.Now, 9));

        }


        /// <summary>
        /// Simple test suitable for speed testing.
        /// </summary>
        private static void SimpleTest()
        {
            // Setup a test dataset
            Invoices inputInvoices;
            List<Payment> inputPayments;
            GenerateSimpleData(out inputInvoices, out inputPayments);

            // Mark the current time
            long ticks = DateTime.Now.Ticks;

            // Perform the match
            Hits hits = MatchPayment.Hits(inputPayments[0], inputInvoices);

            // Mark the end of computation
            Console.WriteLine("Search complete for " + inputInvoices.Count + " levels");
            Console.WriteLine("Payment " + hits.Payment.Reference + " in the amount of " + hits.Payment.Amount);
            Console.WriteLine("Time taken: " + (DateTime.Now.Ticks - ticks) / 10000000.0 + " seconds");
            Console.WriteLine();

            // Write all hits.
            Console.WriteLine("Hits");
            hits.Write();

            Console.WriteLine();

            // Write out the best hit.
            Console.WriteLine("Best hit");
            Hit bestHit = hits.BestHit();
            bestHit.Write();

            Console.WriteLine();

            // Write all unmatched Invoices.
            Console.WriteLine("Unmatched invoices");
            inputInvoices.Remove(bestHit);
            inputInvoices.Write();

            // Wait for keypress to exit
            Console.ReadLine();        
        }


        /// <summary>
        /// Generate random data set.
        /// </summary>
        /// <param name="invoices">Out parameter for Invoices generated.</param>
        /// <param name="payments">Out parameter for payments.</param>
        private static void GenerateRandomData(out Invoices invoices, out List<Payment> payments)
        {
            invoices = new Invoices();
            payments = new List<Payment>();

            int serial = 1;
            Random random = new Random(Convert.ToInt32(DateTime.Now.Ticks % 2 ^ 31));
            
            // Generate the invoices.
            for (int i = 0; i < 1000; i++)
            {
                invoices.Add(new Invoice(serial++, DateTime.Now.AddDays(i), random.Next(100, 1000)));
            }

            decimal sum = 0;
            int matches = 0;
            int invoicesToMatch = -1;
            int invoicesLeft = 0;
            int paymentId = 0;
            for (int i = 0; i < invoices.Count; i++)
            {
                if (sum == 0 || matches == invoicesToMatch)
                {
                    // Determine the nubmer of invoices to match.
                    invoicesToMatch = random.Next(1, 15);
                    invoicesLeft = invoices.Count - i - 1;
                    invoicesToMatch = (invoicesLeft < invoicesToMatch) ? invoicesLeft : invoicesToMatch;
                    matches = 0;
                    sum = 0;
                }

                // Determine if this invoice is included.
                if (random.Next(100) < 90)
                {
                    sum += invoices[i].Amount;
                    matches++;
                }

                // We have a match. Create a Payment with the date of the last invoice.
                if (matches == invoicesToMatch)
                {
                    // Decide if we want to make this Invoice non matchable.
                    if (random.Next(100) > 90)
                    {
                        sum += 0.1M;
                    }
                    payments.Add(new Payment(paymentId.ToString(), invoices[i].Date, sum));
                }
            }
        }


        private static void RandomTest()
        {
            // Setup a test dataset
            Invoices inputInvoices;
            List<Payment> inputPayments;
            GenerateRandomData(out inputInvoices, out inputPayments);

            // Write out the input invoice set
            Console.WriteLine("Invoices to match");
            inputInvoices.Write();
            Console.WriteLine("Number of payments " + inputPayments.Count);
            Console.WriteLine();



            // Match all invoices against all payments;
            for (int i = 0; i < inputPayments.Count; i++)
            {
                // Write out the payment details.
                Console.WriteLine("Matches against Payment "
                    + inputPayments[i].Date
                    + " ID:" + inputPayments[i].Reference
                    + " in the amount of " + inputPayments[i].Amount
                    + " against an input set of " + inputInvoices.Count + " invoices"
                );

                // Perform the match
                Invoices invoicesToMatch = inputInvoices.GetUpToDate(inputPayments[i].Date);
                Hits hits = MatchPayment.Hits(inputPayments[i], invoicesToMatch);
                Console.WriteLine("Found " + hits.Count + " hits. Attempting best hit");
                Hit bestHit = hits.BestHit();


                if (bestHit != null)
                {
                    // Remove the hits from the match list.
                    inputInvoices.Remove(bestHit);

                    // Write out the match.
                    bestHit.Write();
                }
                else
                {
                    Console.WriteLine("*** There was no match against this payment");                    
                }

                Console.WriteLine();
            }

            // Write all unmatched Invoices.
            Console.WriteLine("Unmatched invoices");
            inputInvoices.Write();

            // Wait for keypress to exit
            Console.WriteLine();
            Console.WriteLine("Press Return to exit");
            Console.ReadLine();        

        }

        
        /// <summary>
        /// Test harness to show how the MatchSum function works.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // SimpleTest();
            RandomTest();
        }
    }
}
