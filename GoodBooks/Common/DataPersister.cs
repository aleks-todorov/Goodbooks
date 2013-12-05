using GoodBooks.Models;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoodBooks.Common
{
    public static class DataPersister
    {
        private const string DeveloperKey = "h9UkRSEe06SXyhbcazF6VA";
        private const string BaseUrl = "http://www.goodreads.com/";

        public static async Task<ObservableCollection<SearchResultBookModel>> GetBooks(string word)
        {
            Task.Delay(1000);

            //http://www.goodreads.com/search.xml?key=h9UkRSEe06SXyhbcazF6VA&q=Game

            var url = BaseUrl + "search.xml?key=" + DeveloperKey + "&q=" + word;

            var responseText = await GetResponceData(url);

            if (!string.IsNullOrEmpty(responseText))
            {
                var result = await SearchResultBookModel.ParseBooks(responseText);
                return result;
            }

            return new ObservableCollection<SearchResultBookModel>();
        }


        public static async Task<AuthorFullModel> GetAuthor(string name)
        {
            Task.Delay(1000);
            var id = await GetAuthorId(name);

            if (id != string.Empty)
            {
                var url = BaseUrl + "author/show/" + id + ".xml?key=" + DeveloperKey;

                var responseText = await GetResponceData(url);

                var author = await AuthorFullModel.ParseAuthor(responseText);

                return author;
            }

            return new AuthorFullModel();
        }

        public static async Task<AuthorFullModel> GetAuthorById(int id)
        {
            Task.Delay(1000);
            var url = BaseUrl + "author/show/" + id + ".xml?key=" + DeveloperKey;

            var responseText = await GetResponceData(url);

            if (responseText != string.Empty)
            {
                var author = await AuthorFullModel.ParseAuthor(responseText);

                return author;
            }
            return new AuthorFullModel();
        }

        private static async Task<string> GetAuthorId(string name)
        {
            Task.Delay(1000);
            var url = BaseUrl + "api/author_url/" + name + "?key=" + DeveloperKey;

            var responseText = await GetResponceData(url);

            if (!string.IsNullOrEmpty(responseText))
            {
                var id = await AuthorFullModel.ParseAuthorId(responseText);

                return id;
            }

            return string.Empty;
        }

        private static async Task<string> GetResponceData(string url)
        {
            try
            {
                Task.Delay(1000);
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);

                if (response.ReasonPhrase == "OK")
                {
                    var responseText = await response.Content.ReadAsStringAsync();

                    return responseText.ToString();
                }
            }

            catch (Exception ex)
            {
                //App starts with requests. Since they can be 404. I cannot start spam user just because the site returned 404 on request for random author. 
                //That's why it is better to return empty string and handle the real exceptions elsewhere! 
                //By doing so, the app runns perfectly normal, and when there is real exception it is handled correctly!
                return string.Empty;
            }

            return string.Empty;
        }


        public static async Task<BookPartialModel> GetBook(string bookName)
        {
            Task.Delay(1000);

            var titleAsArray = bookName.Split(new char[] { ' ', ',', '.', '!', '-', '?' }, System.StringSplitOptions.RemoveEmptyEntries);

            var title = new StringBuilder();

            foreach (var item in titleAsArray)
            {
                title.Append(item);
                title.Append("+");
            }

            title.Length--;

            var url = BaseUrl + "book/title.xml?key=" + DeveloperKey + "&title=" + title.ToString();

            var responseText = await GetResponceData(url);

            var book = await BookPartialModel.ParseBook(responseText);

            return book;
        }

        public static async Task<string> GetQuote()
        {
            HttpClient client = new HttpClient();

            var url = "http://feeds.feedburner.com/brainyquote/QUOTEBR";

            try
            {
                var response = await client.GetAsync(url);

                var responseText = await response.Content.ReadAsStringAsync();

                return responseText.ToString();
            }
            catch (HttpRequestException ex)
            {
                new Windows.UI.Popups.MessageDialog(ex.Message + "Please check your internet connection and try again!").ShowAsync();
            }
            return string.Empty;
        }
    }
}
