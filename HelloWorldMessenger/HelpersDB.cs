using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;

namespace HelloWorldMessenger
{
    [Table("Messages")]
    public class MessagesTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public long Message_ID { get; set; }
        public string Login { get; set; }
        public long Dialog_ID { get; set; }
        public long Time { get; set; }
        public string Text { get; set; }
    }

    [Table("Dialogs")]
    public class DialogsTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public long Dialog_ID { get; set; }
        public string Name { get; set; }
        public long Time { get; set; }
        public string Members { get; set; }
        public string Login { get; set; }
    }


    public static class HelpersDB
    {

        static SQLiteConnection db = null;

        //подключниение к БД
        static void ConnectToDB()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HelloWorldDB.db3");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<MessagesTable>();
            db.CreateTable<DialogsTable>();
        }

        //получить сообщения из БД в виде списка
        public static List<MessageData> GetMessages(long dialog_id)
        {
            if (db == null) ConnectToDB();
            TableQuery<MessagesTable> messages = db.Table<MessagesTable>().Where((m) => m.Dialog_ID == dialog_id);

            List<MessageData> result = new List<MessageData>();

            foreach (MessagesTable item in messages)
            {
                result.Add(new MessageData(item.Message_ID, item.Text, item.Login, item.Time));
            }
            return result;
        }


        //поместить сообщения в БД
        public static void PutMessages(List<MessageData> messages, long dialog_id)
        {
            if (db == null) ConnectToDB();

            foreach (MessageData item in messages)
            {
                db.Insert(new MessagesTable() { Login = item.Login, Dialog_ID = dialog_id, Message_ID = item.Id, Text = item.Text, Time = item.Time });

            }
        }


        //поместить диалоги в БД
        public static void PutDialogs(List<DialogData> dialogs, string login)
        {
            if (db == null) ConnectToDB();

            foreach (DialogData item in dialogs)
            {
                db.Insert(new DialogsTable() { Dialog_ID = item.Id, Name = item.Name, Time = item.Time, Members = item.Members, Login = login});

            }
        }

        //получить диалоги из БД по логину
        public static List<DialogData> GetDialogs(string login)
        {
            if (db == null) ConnectToDB();
            TableQuery<DialogsTable> dialogs = db.Table<DialogsTable>().Where(m => m.Login == login);

            List<DialogData> result = new List<DialogData>();

            foreach (DialogsTable item in dialogs)
            {
                result.Add(new DialogData(item.Dialog_ID, item.Name, item.Members, item.Time));

            }
            return result;
        }

        //обновить время диалога в БД
        public static void UpdateDialogTime(long dialog_id, long time)
        {
            if (db == null) ConnectToDB();

            DialogsTable dialog = db.Table<DialogsTable>().First((m) => m.Dialog_ID == dialog_id);
            dialog.Time = time;
            db.Update(dialog);
        }


        //удалить диалог из БД
        public static void DeleteDialogs(string login)
        {
            if (db == null) ConnectToDB();

            TableQuery<DialogsTable> dialogs = db.Table<DialogsTable>().Where((m) => m.Login == login);
            foreach (DialogsTable item in dialogs)
            {
                db.Delete(item);
            }
        }
    }
}