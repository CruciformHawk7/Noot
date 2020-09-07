using System.Data.SqlTypes;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using Noot.Models;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Noot.Controllers {
    public class NotesController {
        private List<Note> Notes = new List<Note>();
        private readonly ISyncLocalStorageService LocalStorage;
        private readonly IJSRuntime _jsRuntime;

        public void CreateNewNote(string Title, string Note) {
            var note = new Note {
                Id = GetRandomString(5),
                Title = Title?? "",
                Notes = Note ?? ""
            };
            Notes.Add(note);
            LocalStorage.SetItem("notes", Notes);
        }

        public NotesController(List<Note> notes, ISyncLocalStorageService localStorage) {
            LocalStorage = localStorage;
            var te = localStorage.GetItem<List<Note>>("notes");
            Notes = te ?? new List<Note>();
        }

        private string GetRandomString(int size = 13) {
            return new string(Enumerable.Repeat("abcdef1234567890", size)
                .Select(s => {
                    var cryptoResult = new byte[4];
                    using (var cryptoProvider = new RNGCryptoServiceProvider())
                        cryptoProvider.GetBytes(cryptoResult);

                    return s[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(s.Length)];
                })
                .ToArray());
        }

        public List<Note> GetAllNotes() {
            return Notes;
        }

        public void RemoveNote(string Id) {
            Notes.RemoveAll(e => e.Id == Id );
            LocalStorage.SetItem("notes", Notes);
        }

        public Note GetNote(string Id) {
            return Notes.Find(e => e.Id == Id);
        }

        public void EditNote(string Id, string Title, string Note) {
            var N = Notes.Find(e => e.Id == Id);
            N.Notes = Note;
            N.Title = Title;
            LocalStorage.SetItem("notes", Notes);
        }
    }
}