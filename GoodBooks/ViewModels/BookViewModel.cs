using GoodBooks.Behavior;
using GoodBooks.Common;
using GoodBooks.Models;
using System;
using System.Linq;
using System.Windows.Input;

namespace GoodBooks.ViewModels
{
    public class BookViewModel : BindableBase
    {
        private ICommand getBook;
        private BookPartialModel book;
        public bool IsInProgress { get; set; }

        public BookPartialModel Book
        {
            get
            {
                if (this.book == null)
                {
                    book = new BookPartialModel();
                }

                return book;
            }

            set
            {
                if (this.book == null)
                {
                    this.book = new BookPartialModel();
                }

                book = value;
                this.OnPropertyChanged("Book");
            }
        }

        public ICommand GetBook
        {
            get
            {
                if (this.getBook == null)
                {
                    this.getBook = new RelayCommand(
                        this.HandleExecuteGetBookCommand);
                }
                return this.getBook;
            }
        }

        private async void HandleExecuteGetBookCommand(object parameter)
        {
            if (!String.IsNullOrEmpty(parameter.ToString()))
            {
                this.book = new BookPartialModel();
                StartRing();
                HideNotifications();
                var bookName = parameter.ToString();
                this.Book = await DataPersister.GetBook(bookName);
                this.OnPropertyChanged("Book");
                StopRing();
                if (this.Book.Title == null)
                {
                    ShowNotification();
                }
            }
        }

        public async void ChangeBook(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.Book = await DataPersister.GetBook(title);
                this.OnPropertyChanged("Book");
            }
            else
            {
                this.book.Description = "No book found";
            }
        }

        private void HideNotifications()
        {
            this.ResultsFound = false;
            OnPropertyChanged("ResultsFound");
        }

        private void ShowNotification()
        {
            this.ResultsFound = true;
            OnPropertyChanged("ResultsFound");
        }

        private void StopRing()
        {
            this.IsInProgress = false;
            this.OnPropertyChanged("IsInProgress");
        }

        private void StartRing()
        {
            this.IsInProgress = true;
            this.OnPropertyChanged("IsInProgress");
        }

        public bool ResultsFound { get; set; }
    }
}
