using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using Notes.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Notes.ViewModels;

public partial class AllNotesViewModel : IQueryAttributable
{
    public ObservableCollection<NoteViewModel> AllNotes { get; }

    private INoteService _noteService;
    public AllNotesViewModel(INoteService noteService)
    {
        _noteService = noteService;
        AllNotes = new ObservableCollection<NoteViewModel>(_noteService.GetItems().Select(n => new NoteViewModel(_noteService, n)));
    }

    [RelayCommand]
    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    [RelayCommand]
    private async Task SelectNoteAsync(NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Id == int.Parse(noteId)).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Id == int.Parse(noteId)).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new NoteViewModel(_noteService, _noteService.GetItem(int.Parse(noteId))));
        }
    }
}