using GoodBooks.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBooks.ViewModels
{
    public class FaqViewModel
    {
        private ObservableCollection<FaqModel> results;

        public IEnumerable<FaqModel> Results
        {
            get
            {
                if (results == null)
                {
                    this.results = new ObservableCollection<FaqModel>();
                }
                return results;
            }

            set
            {
                if (this.results == null)
                {
                    this.results = new ObservableCollection<FaqModel>();
                }

                foreach (var item in value)
                {
                    this.results.Add(item);
                }
            }
        }
        public FaqViewModel()
        {
            this.Results = FaqModel.GetQuestions();
        }

    }
}
