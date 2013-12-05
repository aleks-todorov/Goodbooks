using System.Collections.ObjectModel;
namespace GoodBooks.Models
{
    public class FaqModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        private FaqModel(string question, string answer)
        {
            this.Question = "Q: " + question;
            this.Answer = "A: " + answer;
        }

        internal static System.Collections.Generic.IEnumerable<FaqModel> GetQuestions()
        {
            var questions = new ObservableCollection<FaqModel>();

            string question;
            string answer;
            FaqModel faq;

            question = "What is this program used for?";
            answer = "This program is used for searching books, authors, and making notes about them. It is very useful for everybody who love reading and want to keep track of their books.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "Can I use this program offline?";
            answer = "No, it uses the public services of goodreads and in order to obtain the required information.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "How do I search for specific author or book?";
            answer = "You can search for specific author or book, by going to the corresponding section and use the search fields.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "Why some of the authors do not have profile picture or books?";
            answer = "Unfortunately this is not a question to which I can answer. All the information provided from goodreads is consumed and displayed.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "How do I search for book even when I am not sure about the full name?";
            answer = "You can search books for keyword/s by using the Windows 8 search option from the right command panel(charms).";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "I want ot print some information, but after clicking the print button don't know how to continue.";
            answer = "By click 'Print Info' buttons, you are redirected to print preview page. If you want to print, you will have to select the print device from the Devices charm.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "Why do I need to save notes as template?";
            answer = "By saving Notes to template you can easily send it to some of your friends, and when they import it in the program, they can see all notes you wanted them to have. Also it is very useful for making manual backups before Windows re-installing for example.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            question = "Are there any updates planed for this app?";
            answer = "Yes, as soon as I have any free time, I will extend the functionality of the application with the rest of the options goodreads offer- shelving books, following authors, commenting and rating books, etc.";
            faq = new FaqModel(question, answer);
            questions.Add(faq);

            return questions;
        }
    }
}
