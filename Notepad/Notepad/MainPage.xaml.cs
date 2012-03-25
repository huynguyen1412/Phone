using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Notepad {
    public partial class MainPage :PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();

            this.DataContext = NotepadViewModel.Instance;
            ucNoteList.DataContext = NotepadViewModel.Instance;
            ucUserRegistration.DataContext = NotepadViewModel.Instance;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {

            if(!string.IsNullOrEmpty(txtNote.Text)) {
                NotepadViewModel.Instance.SaveNote(txtNoteName.Text, txtNote.Text);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            NotepadViewModel.Instance.DeleteNote();

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e) {
            NotepadViewModel.Instance.SelectedNote = null;

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            if(!string.IsNullOrEmpty(txtNote.Text)) {
                NotepadViewModel.Instance.SaveNote(txtNoteName.Name, txtNote.Text);
            }
        }
    }
}