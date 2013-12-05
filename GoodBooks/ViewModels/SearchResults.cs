using GoodBooks.Common;
using GoodBooks.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GoodBooks.ViewModels
{
    class SearchResults : BindableBase
    {
        private string queryText = "";
        public bool ResultsFound { get; set; }
        public bool IsInProgress { get; set; }

        public SearchResults()
        {
            this.ResultsFound = false;
            this.IsInProgress = true;
        }

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.queryText = value;
                this.OnPropertyChanged("QueryText");
                this.ResultsFound = true;
                this.OnPropertyChanged("ResultsFound");
                this.IsInProgress = true;
                this.LoadResults();
            }
        }

        private ObservableCollection<SearchResultBookModel> results;
        public IEnumerable<SearchResultBookModel> Results
        {
            get
            {
                if (this.results == null)
                {
                    results = new ObservableCollection<SearchResultBookModel>();
                }

                return results;
            }
            set
            {
                if (this.results == null)
                {
                    this.results = new ObservableCollection<SearchResultBookModel>();
                }
                this.results.Clear();

                foreach (var item in value)
                {
                    this.results.Add(item);
                }
            }
        }

        private async void LoadResults()
        {
            this.Results = await DataPersister.GetBooks(this.QueryText);

            if (results.Count > 0)
            {
                this.ResultsFound = false;
            }
            else
            {
                this.ResultsFound = true;
            }

            this.IsInProgress = false;
            OnPropertyChanged("Results");
            OnPropertyChanged("ResultsFound");
            OnPropertyChanged("IsInProgress");
        }
    }
}
