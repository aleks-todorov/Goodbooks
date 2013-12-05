using GoodBooks.Behavior;
using GoodBooks.Common;
using GoodBooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GoodBooks.ViewModels
{
    public class NotesViewModel : BindableBase
    {
        private ObservableCollection<MyNotes> notes;
        private ICommand addNoteCommand;
        private ICommand deleteNoteCommand;

        private MyNotes newNote;

        public NotesViewModel()
        {
            this.notes = new ObservableCollection<MyNotes>();
            this.newNote = new MyNotes();
        }

        public IEnumerable<MyNotes> MyNotes
        {
            get
            {
                if (this.notes == null)
                {
                    this.notes = new ObservableCollection<MyNotes>();
                }

                return this.notes;
            }

            set
            {
                if (this.notes == null)
                {
                    this.notes = new ObservableCollection<MyNotes>();
                }

                this.SetObservableValues(this.notes, value);
            }
        }

        public MyNotes NewNote
        {
            get
            {
                return this.newNote;
            }
            set
            {
                if (this.newNote != value)
                {
                    this.newNote = value;
                    this.OnPropertyChanged("NewNote");
                }
            }
        }

        public ICommand AddNote
        {
            get
            {
                if (this.addNoteCommand == null)
                {
                    this.addNoteCommand = new RelayCommand(this.HandleAddNoteCommand);
                }

                return this.addNoteCommand;
            }
        }

        public ICommand DeleteNote
        {
            get
            {
                if (this.deleteNoteCommand == null)
                {
                    this.deleteNoteCommand = new RelayCommand(this.HandleDeleteNoteCommand);
                }

                return this.deleteNoteCommand;
            }
        }

        private void HandleDeleteNoteCommand(object parameter)
        {
            if (!string.IsNullOrEmpty(this.newNote.Title))
            {
                var note = this.notes.FirstOrDefault(n => n.Title == newNote.Title && n.Content == newNote.Content);
                if (note != null)
                {
                    this.notes.Remove(note);
                }

                this.newNote = new MyNotes();
                OnPropertyChanged("NewNote");
                BackupNotes();
            }
        }

        private void HandleAddNoteCommand(object parameter)
        {
            if (!string.IsNullOrEmpty(this.newNote.Title))
            {
                this.newNote.DateCreated = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss tt");
                this.notes.Add(this.newNote);
                this.NewNote = new MyNotes();
                OnPropertyChanged("NewNote");
                BackupNotes();
            }
        }

        private void BackupNotes()
        {
            var json = JsonConvert.SerializeObject(this.notes);
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["notesBackup"] = json;
            if (json.Length < 90000)
            {
                var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["notesBackup"] = json;
            }
        }

        public void LoadMyNotes(IEnumerable<MyNotes> value)
        {
            this.MyNotes = value;
            BackupNotes();
        }

        private void SetObservableValues<T>(ObservableCollection<T> observableCollection, IEnumerable<T> values)
        {
            if (observableCollection != values)
            {
                observableCollection.Clear();
                foreach (var item in values)
                {
                    observableCollection.Add(item);
                }
            }
        }
    }
}
