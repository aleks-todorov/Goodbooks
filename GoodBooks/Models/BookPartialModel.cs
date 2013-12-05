using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace GoodBooks.Models
{
    public class BookPartialModel
    {
        public string BookId { get; set; }
        public string ISBN { get; set; }
        public string TextReviewCount { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string Link { get; set; }
        public string NumberOfPages { get; set; }
        public string Publisher { get; set; }
        public string PublishedYear { get; set; }
        public string AverageRating { get; set; }
        public string RatingsCount { get; set; }
        public string Description { get; set; }
        public ICollection<AuthorFullModel> Authors { get; set; }
        public ICollection<BookPartialModel> SimilarBooks { get; set; }

        public BookPartialModel()
        {
            this.Authors = new List<AuthorFullModel>();
            this.SimilarBooks = new List<BookPartialModel>();
        }

        public static async Task<BookPartialModel> ParseBook(string text)
        {
            XmlDocument books = new XmlDocument();

            if (string.IsNullOrEmpty(text))
            {
                var bookNotFound = new BookPartialModel();
                return bookNotFound;
            }

            books.LoadXml(text);

            var collection = books.GetElementsByTagName("book");

            var bookModel = new BookPartialModel();

            foreach (var item in collection)
            {
                foreach (var bookProperties in item.ChildNodes)
                {
                    if (bookProperties.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                    {
                        continue;
                    }

                    switch (bookProperties.NodeName)
                    {
                        case "id": bookModel.BookId = bookProperties.InnerText; break;
                        case "isbn": bookModel.ISBN = bookProperties.InnerText; break;
                        case "text_reviews_count": bookModel.TextReviewCount = bookProperties.InnerText; break;
                        case "title": bookModel.Title = bookProperties.InnerText; break;
                        case "image_url": bookModel.ImageUrl = bookProperties.InnerText; break;
                        case "small_image_url": bookModel.SmallImageUrl = bookProperties.InnerText; break;
                        case "link": bookModel.Link = bookProperties.InnerText; break;
                        case "num_pages": bookModel.NumberOfPages = bookProperties.InnerText; break;
                        case "publisher": bookModel.Publisher = bookProperties.InnerText; break;
                        case "publication_year": bookModel.PublishedYear = bookProperties.InnerText; break;
                        case "average_rating": bookModel.AverageRating = bookProperties.InnerText; break;
                        case "ratings_count": bookModel.RatingsCount = bookProperties.InnerText; break;
                        case "description": bookModel.Description = bookProperties.InnerText; break;
                        case "authors":
                            {
                                foreach (var bookAuthor in bookProperties.ChildNodes)
                                {
                                    if (bookAuthor.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                    {
                                        continue;
                                    }

                                    var inBookAuthor = new AuthorFullModel();

                                    foreach (var props in bookAuthor.ChildNodes)
                                    {
                                        if (props.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                        {
                                            continue;
                                        }

                                        switch (props.NodeName)
                                        {
                                            case "id": inBookAuthor.Id = props.InnerText; break;
                                            case "name": inBookAuthor.Name = props.InnerText; break;
                                            case "image_url": inBookAuthor.ImageUrl = props.InnerText; break;
                                            case "small_image_url": inBookAuthor.SmallImageUrl = props.InnerText; break;
                                            case "link": inBookAuthor.Link = props.InnerText; break;
                                        }
                                    }

                                    bookModel.Authors.Add(inBookAuthor);
                                }

                            }; break;
                        case "similar_books":
                            {
                                foreach (var similarBook in bookProperties.ChildNodes)
                                {
                                    if (similarBook.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                    {
                                        continue;
                                    }

                                    var exampleBook = new BookPartialModel();

                                    foreach (var similarBookProp in similarBook.ChildNodes)
                                    {

                                        if (similarBookProp.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                        {
                                            continue;
                                        }

                                        switch (similarBookProp.NodeName)
                                        {
                                            case "id": exampleBook.BookId = similarBookProp.InnerText; break;
                                            case "isbn": exampleBook.ISBN = similarBookProp.InnerText; break;
                                            case "text_reviews_count": exampleBook.TextReviewCount = similarBookProp.InnerText; break;
                                            case "image_url": exampleBook.ImageUrl = similarBookProp.InnerText; break;
                                            case "small_image_url": exampleBook.SmallImageUrl = similarBookProp.InnerText; break;
                                            case "averate_rating": exampleBook.AverageRating = similarBookProp.InnerText; break;
                                            case "ratings_count": exampleBook.RatingsCount = similarBookProp.InnerText; break;
                                            case "title": exampleBook.Title = similarBookProp.InnerText; break;
                                            case "authors":
                                                {
                                                    foreach (var similarBookAuthor in similarBookProp.ChildNodes)
                                                    {
                                                        if (similarBookAuthor.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                                        {
                                                            continue;
                                                        }

                                                        var similarAuthor = new AuthorFullModel();

                                                        foreach (var similarProps in similarBookAuthor.ChildNodes)
                                                        {
                                                            if (similarProps.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                                            {
                                                                continue;
                                                            }

                                                            switch (similarProps.NodeName)
                                                            {
                                                                case "id": similarAuthor.Id = similarProps.InnerText; break;
                                                                case "name": similarAuthor.Name = similarProps.InnerText; break;
                                                            }
                                                        }

                                                        exampleBook.Authors.Add(similarAuthor);
                                                    }
                                                } break;
                                        }
                                    }

                                    bookModel.SimilarBooks.Add(exampleBook);
                                }
                            } break;
                    }
                }
                break;
            }
            return bookModel;
        }
    }
}
