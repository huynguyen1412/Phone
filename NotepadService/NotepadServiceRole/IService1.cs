using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace NotepadServiceRole {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotepadService" in both code and config file together.
    [ServiceContract]
    public interface INotepadService {

        [OperationContract]
        Guid AddUser(Guid userId, string userName);

        [OperationContract]
        NoteDto AddNote(Guid userId, string noteDescription, string noteText);

        [OperationContract]
        void UpdateNote(int noteId, string noteText);

        [OperationContract]
        void DeleteNote(Guid userId, int noteId);

        [OperationContract]
        List<NoteDto> GetNotes(Guid userId);

        [OperationContract]
        NoteDto GetNote(Guid userId, int noteId);
    }
}
