namespace Feefo.Requests
{
    public enum Mode
    {
        None = 0,
        /// <summary>
        /// Returns the supplier’s service feedback only.
        /// </summary>
        Service,
        /// <summary>
        /// Returns the supplier’s product feedback only
        /// </summary>
        Product,
        /// <summary>
        /// Returns both the service and product feedback for a supplier.
        /// </summary>
        ServiceAndProduct,
        /// <summary>
        ///  Returns the supplier’s product feedback. If every product has a
        ///  customer comment associated with the rating, the output for
        ///  mode = productonly data will be the same as for mode = product.
        ///  Negative ratings require a comment, positive ratings do not. For
        ///  those products where there is no comment, mode = productonly
        ///  converts the positive rating to its equivalent text value (i.e. Excellent
        ///  or Good) and outputs this as the product comment.
        /// </summary>
        ProductOnly,
        /// <summary>
        /// Returns the supplier’s product feedback but only where the
        /// customer has left comments for the product. As customers tend to
        /// leave more comments when the product is rated as Poor or Bad,
        /// mode = productco will generally result in a disproportionate number
        /// of negative ratings being listed.
        /// </summary>
        ProductCo,
    }
}