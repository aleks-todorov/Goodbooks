using GoodBooks.Common;
using GoodBooks.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace GoodBooks.ViewModels
{
    public class GoodbooksViewModel : BindableBase
    {
        private Random randomGenerator = new Random();

        private QuoteModel quote;
        private ObservableCollection<AuthorFullModel> randomAuthors;

        public GoodbooksViewModel()
        {
            GetQuote();
            GetRandomAuthors();
        }

        private async void GetRandomAuthors()
        {
            var list = new ObservableCollection<AuthorFullModel>();

            while (list.Count < 24)
            {
                var id = randomGenerator.Next(1, 2000);

                var randomAuthor = await DataPersister.GetAuthorById(id);

                //if (randomAuthor.Name == null || randomAuthor.ImageUrl.Contains("nophoto"))
                if (randomAuthor.Name == null)
                {
                    continue;
                }

                list.Add(randomAuthor);

                if (list.Count % 4 == 0)
                {
                    this.RandomAuthors = list;
                }
            }
        }

        public IEnumerable<AuthorFullModel> RandomAuthors
        {
            get
            {
                if (this.randomAuthors == null)
                {
                    this.randomAuthors = new ObservableCollection<AuthorFullModel>();
                }

                return randomAuthors;
            }

            set
            {
                if (this.randomAuthors == null)
                {
                    this.randomAuthors = new ObservableCollection<AuthorFullModel>();
                }

                this.randomAuthors.Clear();
                foreach (var item in value)
                {
                    randomAuthors.Add(item);
                }

                this.OnPropertyChanged("RandomAuthors");
            }
        }

        public QuoteModel Quote
        {
            get
            {
                if (this.quote == null)
                {
                    this.quote = new QuoteModel();
                }

                return quote;
            }

            set
            {
                if (this.quote == null)
                {
                    this.quote = new QuoteModel();
                }

                quote = value;
                this.OnPropertyChanged("Quote");
            }
        }

        private async void GetQuote()
        {
            var text = await DataPersister.GetQuote();
            if (!string.IsNullOrEmpty(text))
            {
                this.quote = QuoteModel.ParseQuote(text);
                this.OnPropertyChanged("Quote");
            }
            else
            {
                this.quote = new QuoteModel();
                quote.Description = "Error with the network connection!";
                quote.Title = "Please check and try again!";
            }
        }
    }
}
