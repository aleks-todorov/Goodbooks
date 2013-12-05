using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace GoodBooks.Models
{
    public class AuthorFullModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FansCount { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string About { get; set; }
        public string WorksCount { get; set; }
        public string Gender { get; set; }
        public string Hometown { get; set; }
        public string BornAt { get; set; }
        public string DiedAt { get; set; }
        public ICollection<BookPartialModel> Books { get; set; }

        public AuthorFullModel()
        {
            this.Books = new List<BookPartialModel>();
        }

        public static async Task<string> ParseAuthorId(string text)
        {
            var id = string.Empty;
            XmlDocument authors = new XmlDocument();
            authors.LoadXml(text);

            var collection = authors.GetElementsByTagName("author");

            foreach (var item in collection)
            {
                foreach (var atr in item.Attributes)
                {
                    if (atr.LocalName.ToString() == "id")
                    {
                        id = atr.InnerText;
                    }
                }
            }

            return id;
        }

        public static async Task<AuthorFullModel> ParseAuthor(string text)
        {
            var author = new AuthorFullModel();

            XmlDocument authors = new XmlDocument();
            authors.LoadXml(text);

            var collection = authors.GetElementsByTagName("author");

            foreach (var item in collection)
            {
                foreach (var subItem in item.ChildNodes)
                {
                    if (subItem.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                    {
                        continue;
                    }

                    switch (subItem.NodeName)
                    {
                        case "id": author.Id = subItem.InnerText; break;
                        case "name": author.Name = subItem.InnerText; break;
                        case "link": author.Link = subItem.InnerText; break;
                        case "fans_count": author.FansCount = subItem.InnerText; break;
                        case "image_url": author.ImageUrl = subItem.InnerText; break;
                        case "small_image_url": author.SmallImageUrl = subItem.InnerText; break;
                        case "about": author.About = subItem.InnerText; break;
                        case "works_count": author.WorksCount = subItem.InnerText; break;
                        case "gender": author.Gender = subItem.InnerText; break;
                        case "hometown": author.Hometown = subItem.InnerText; break;
                        case "born_at": author.BornAt = subItem.InnerText; break;
                        case "died_at": author.DiedAt = subItem.InnerText; break;
                        case "books":
                            {
                                foreach (var book in subItem.ChildNodes)
                                {
                                    if (book.GetType() == typeof(Windows.Data.Xml.Dom.XmlText))
                                    {
                                        continue;
                                    }

                                    var bookModel = new BookPartialModel();

                                    foreach (var bookProperties in book.ChildNodes)
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
                                            case "averate_rating": bookModel.AverageRating = bookProperties.InnerText; break;
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
                                        }
                                    }

                                    author.Books.Add(bookModel);
                                }
                            } break;
                    }
                }
            }
            return author;
        }
    }
}
