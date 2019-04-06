using System;

using Speechless.Mobile.SpeechlessApp.Models;

namespace Speechless.Mobile.SpeechlessApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
