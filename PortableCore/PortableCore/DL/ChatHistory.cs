using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
{
    public class ChatHistory : IBusinessEntity
    {
        public ChatHistory()
        {

        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int ID { get; set; }
        [Indexed]
        public int ChatID { get; set; }
        public string TextTo { get; set; }//введенный пользователем текст
        public string TextFrom { get; set; }//ответ робота
        public string Transcription { get; set; }//траскрипция
        public string Definition { get; set; }//часть речи
        public bool InFavorites { get; set; }//добавлен в избранное
        public int RequestStatus { get; set; }//статус запроса к сервису, для отображения признака ожидания ответа
        public int LanguageTo { get; set; }//язык на который переводим, может отличаться от направления, заданного для чата, поскольку пользователь может вводить на любом языке чата
        public int LanguageFrom { get; set; }//аналогично LanguageTo
        [Indexed]
        public DateTime UpdateDate { get; set; }
        public int DeleteMark { get; set; }
        [Indexed]
        public int ParentRequestID { get; set; }
    }
}