using MixERP.Net.VCards;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Specifies a business card that a user can share with others.
    /// </summary>
    public interface IBusinessCard
    {
        /// <summary>
        /// Gets or sets a custom label (human-friendly identifier) for the business card.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Imports information from the specified business card. 
        /// </summary>
        /// <param name="other">The business card to import from.</param>
        void Import(IBusinessCard other);
    }
}