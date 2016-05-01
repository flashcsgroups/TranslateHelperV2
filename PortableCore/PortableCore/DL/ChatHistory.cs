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
        public int InFavorites { get; set; }//добавлен в избранное
        public int RequestStatus { get; set; }//статус запроса к сервису, для отображения признака ожидания ответа
        [Indexed]
        public DateTime UpdateDate { get; set; }
        public int DeleteMark { get; set; }
    }
}