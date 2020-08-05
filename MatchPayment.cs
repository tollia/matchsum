using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceMatcher
{
    class MatchPayment
    {
        /// <summary>
        /// The MatchSum FindSoluions entry point. This is used to set up for the recursion and hide
        /// the uninteresting parameter details of the Recurse routine from users.
        /// </summary>
        /// <param name="targetValue">The sum being matched.</param>
        /// <param name="inputValues">A List of match against targetValue.</param>
        /// <returns>List of List of decimal. Each sub list constitutes one match.</returns>
        public static Hits Hits(Payment payment, Invoices inputValues)
        {
            // Protect against invalid parameters
            if (inputValues == null || inputValues.Count == 0)
            {
                throw new ArgumentException("Please pass in a valid list of decimals.");
            }

            if (payment.Amount <= 0)
            {
                throw new ArgumentException("The targetValue has to be a non zero positive decimal.");
            }

            // Make a copy as we do no wish to disturb shape of the orignal List passed in.
            Invoices values = new Invoices(inputValues);

            // Sort in decending order to make the recursion more shallow by adding the 
            // higest values first. There is no descnding sort option so Reverse is simpler
            // than writing a custom IComparer for these relatively small inputs.
            values.Sort();
            values.Reverse();

            // Call the recursive tree search for sums matching target.
            Hits hits = new Hits(payment);
            Recurse recurse = new Recurse(payment.Amount, values, hits);
            recurse.FindHits();

            return hits;
        }


        /// <summary>
        /// This private sub class runs the recursion in a thread safe fashion, taking all the 
        /// input data and storing it in global variables inside the class. This reduces
        /// calling overhead by having the MatchSum method pass fewer parameters hence 
        /// causing less stack push during recursion resulting in a speedup of around 30%. 
        /// If thread safety is not an issue these global variables could just as well
        /// be declared in the parent class and omitting this class all together.
        /// </summary>
        private class Recurse
        {
            /// <summary>
            /// The sum to be searched for.
            /// </summary>
            private decimal targetValue;


            /// <summary>
            /// The values to be summed.
            /// </summary>
            private List<Invoice> values;


            /// <summary>
            /// The structure to collect the results into.
            /// </summary>
            private Hits hits;


            /// <summary>
            /// Holds the current search path but may have trailing data so to solution is the first
            /// level + 1 entries.
            /// </summary>
            private Invoice[] path;


            /// <summary>
            /// Invoice constant with zero amount.
            /// </summary>
            private Invoice ZERO_INVOICE = new Invoice(0, DateTime.Now, 0);


            /// <summary>
            /// Conrstructor for the recurse method for this set of parameters.
            /// </summary>
            /// <param name="targetValue"></param>
            /// <param name="values"></param>
            /// <param name="result"></param>
            public Recurse(decimal targetValue, List<Invoice> values, Hits hits)
            {
                this.targetValue = targetValue;
                this.values = values;
                this.hits = hits;
                this.path = new Invoice[values.Count];
            }


            /// <summary>
            /// Entry point for neatness as the calling method need not know anything about the
            /// inner working of this class.
            /// </summary>
            public void FindHits()
            {
                FindHits(0, 0);
            }


            /// <summary>
            /// The main recursive match routine of MatchSum.
            /// </summary>
            /// <param name="currentValue">The sum up to this point in the recursion.</param>
            /// <param name="level">Depth of recursion for easily retreiving the value for this level from values.</param>
            public void FindHits(decimal currentValue, int level)
            {
                decimal newValue = currentValue + values[level].Amount;

                // If adding the value at this level matches the targetValue we add the current path
                // to the results list.
                if (newValue == targetValue)
                {
                    path[level] = values[level];
                    SolutionFound(level);
                    return;
                }

                // Check if we have reached the bottom of the search tree and thus do not wish to recurse
                // further.
                if (level >= (values.Count - 1))
                {
                    return;
                }

                // Explore subtrees adding value at this level only if it does not exceed
                // the target. If it does exceed, that entire subtree is cut from the search.
                if (newValue < targetValue)
                {
                    path[level] = values[level];
                    FindHits(newValue, level + 1);
                }

                // Explore all subtrees where value at this level is not part of the sum.
                path[level] = ZERO_INVOICE;
                FindHits(currentValue, level + 1);
            }


            /// <summary>
            /// Convenience method to register a valid solution, skipping zeros in the copy of decimals making up the match.
            /// </summary>
            /// <param name="level">Depth in the tree at which a match was found to limit the copy to relevant values</param>
            void SolutionFound(int level)
            {
                Hit hit = new Hit(hits.Payment);
                hits.Add(hit);
                for (int i = 0; i <= level; i++)
                {
                    if (path[i].Amount != 0)
                    {
                        hit.Add(path[i]);
                    }
                }
            }
        }    
    }
}
