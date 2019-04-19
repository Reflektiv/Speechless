using MixERP.Net.VCards;

namespace Reflektiv.Speechless.Core.Domain.Contracts.Models
{
    /// <summary>
    /// Specifies a business card that a user can share with others.
    /// </summary>
    public interface IBusinessCard
    {
        /// <summary>
        /// Gets or sets the human-friendly identifier of the card.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets the details of the business card.
        /// </summary>
        VCard Details { get; set; }

    }
}