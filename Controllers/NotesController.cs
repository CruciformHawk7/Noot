using System.Data.SqlTypes;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using Noot.Models;
using System.Linq;

namespace Noot.Controllers {
    public static class NotesController {
        private static List<Note> Notes = new List<Note>();

        public static void CreateNewNote(string Title, string Note) {
            var note = new Note {
                Id = GetRandomString(5),
                Title = Title?? "",
                Notes = Note ?? ""
            };
            Notes.Add(note);
        }

        static private string GetRandomString(int size = 13) {
            return new string(Enumerable.Repeat("abcdef1234567890", size)
                .Select(s => {
                    var cryptoResult = new byte[4];
                    using (var cryptoProvider = new RNGCryptoServiceProvider())
                        cryptoProvider.GetBytes(cryptoResult);

                    return s[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(s.Length)];
                })
                .ToArray());
        }

        public static List<Note> GetAllNotes() {
            return Notes;
        }

        public static void RemoveNote(string Id) {
            Notes.RemoveAll(e => e.Id == Id );
        }

        public static Note GetNote(string Id) {
            return Notes.Find(e => e.Id == Id);
        }

        public static void EditNote(string Id, string Title, string Note) {
            var N = Notes.Find(e => e.Id == Id);
            N.Notes = Note;
            N.Title = Title;
        }
    }
}