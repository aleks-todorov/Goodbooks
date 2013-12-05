using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace GoodBooks.Models
{
    public class SearchResultBookModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }

        public static async Task<ObservableCollection<SearchResultBookModel>> ParseBooks(string text)
        {
            XmlDocument books = new XmlDocument();

            if (string.IsNullOrEmpty(text))
            {
                var bookNotFound = new SearchResultBookModel();
                new Windows.UI.Popups.MessageDialog("No books found!").ShowAsync();
                return new ObservableCollection<SearchResultBookModel>();
            }

            books.LoadXml(text);

            var booksList = books.GetElementsByTagName("results");

            var results = new ObservableCollection<SearchResultBookModel>();

            foreach (var work in booksList)
            {
                foreach (var bestBook in work.ChildNodes)
                {
                    if (bestBook.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                    {
                        continue;
                    }
                    else if (bestBook.NodeName != "work")
                    {
                        continue;
                    }

                    foreach (var bookExample in bestBook.ChildNodes)
                    {
                        if (bookExample.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                        {
                            continue;
                        }

                        var resultBook = new SearchResultBookModel();
                        if (bookExample.NodeName == "best_book")
                        {
                            foreach (var tag in bookExample.ChildNodes)
                            {
                                switch (tag.NodeName)
                                {
                                    case "title": resultBook.Title = tag.InnerText; break;
                                    case "image_url": resultBook.ImageUrl = tag.InnerText; break;
                                    case "small_image_url": resultBook.SmallImageUrl = tag.InnerText; break;
                                }
                            }
                            results.Add(resultBook);
                        }
                    }
                }
                break;
            }
            return results;
        }
    }
}
