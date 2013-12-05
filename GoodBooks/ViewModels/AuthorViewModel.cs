using GoodBooks.Behavior;
using GoodBooks.Common;
using GoodBooks.Models;
using System.Windows.Input;

namespace GoodBooks.ViewModels
{
    public class AuthorViewModel : BindableBase
    {
        private ICommand getAuthor;
        private AuthorFullModel author = null;
        public bool IsInProgress { get; set; }
        public bool ResultsFound { get; set; }

        public AuthorFullModel Author
        {
            get
            {
                if (this.author == null)
                {
                    author = new AuthorFullModel();
                }

                return author;
            }

            set
            {
                if (this.author == null)
                {
                    this.author = new AuthorFullModel();
                }

                author = value;
                this.OnPropertyChanged("Author");
            }
        }

        public ICommand GetAuthor
        {
            get
            {
                if (this.getAuthor == null)
                {
                    this.getAuthor = new RelayCommand(
                        this.HandleExecuteGetAuthorCommand);
                }
                return this.getAuthor;
            }
        }

        private async void HandleExecuteGetAuthorCommand(object parameter)
        {
            StartRing();
            HideNotifications();
            this.author = new AuthorFullModel();
            this.OnPropertyChanged("Author");
            var authorName = parameter.ToString();
            if (!string.IsNullOrEmpty(parameter.ToString()))
            {
                this.Author = await DataPersister.GetAuthor(authorName);
                this.OnPropertyChanged("Author");
                StopRing();
                if (this.Author.Name == null)
                {
                    ShowNotification();
                }
            }
        }

        public async void GetAuthorByName(string name)
        {
            StartRing();
            if (!string.IsNullOrEmpty(name))
            {
                HideNotifications();
                this.Author = await DataPersister.GetAuthor(name);
                this.OnPropertyChanged("Author");
                StopRing();
                if (this.Author.Name == null)
                {
                    ShowNotification();
                }
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
    }
}
