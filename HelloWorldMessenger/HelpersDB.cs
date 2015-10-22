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
        public bool IsNew { get; set; }
    }


    public static class HelpersDB
    {

        static SQLiteConnection db = null;
        static SQLiteAsyncConnection dbAsync = null;
        
        //подключниение к БД
        static void ConnectToDB()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HelloWorldDB.db3");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<MessagesTable>();
            db.CreateTable<DialogsTable>();
        }

        static void ConnectToDBAsync()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HelloWorldDB.db3");
            dbAsync = new SQLiteAsyncConnection(dbPath);
            dbAsync.CreateTableAsync<MessagesTable>();
            dbAsync.CreateTableAsync<DialogsTable>();
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
            if (dbAsync == null) ConnectToDBAsync();

            List<MessagesTable> items = new List<MessagesTable>();

            foreach (MessageData item in messages)
            {
                items.Add(new MessagesTable() { Login = item.Login, Dialog_ID = dialog_id, Message_ID = item.Id, Text = item.Text, Time = item.Time });
            }

            if (items.Count>0) dbAsync.InsertAllAsync(items);
        }


        //поместить диалоги в БД
        public static void PutDialogs(List<DialogData> dialogs, string login)
        {
            if (dbAsync == null) ConnectToDBAsync();

            List<DialogsTable> items = new List<DialogsTable>();

            foreach (DialogData item in dialogs)
            {
                items.Add(new DialogsTable() { Dialog_ID = item.Id, Name = item.Name, Time = item.Time, Members = item.Members, Login = login, IsNew = item.IsNew });
            }

            if (items.Count > 0) dbAsync.InsertAllAsync(items);
        }

        //получить диалоги из БД по логину
        public static List<DialogData> GetDialogs(string login)
        {
            if (db == null) ConnectToDB();
            TableQuery<DialogsTable> dialogs = db.Table<DialogsTable>().Where(m => m.Login == login);

            List<DialogData> result = new List<DialogData>();

            foreach (DialogsTable item in dialogs)
            {
                result.Add(new DialogData(item.Dialog_ID, item.Name, item.Members, item.Time, item.IsNew));

            }
            return result;
        }

        //обновить время диалога в БД
        public static void UpdateDialogTime(long dialog_id, long time)
        {
            if (dbAsync == null) ConnectToDBAsync();
            if (db == null) ConnectToDB();

            DialogsTable dialog = db.Table<DialogsTable>().First((m) => m.Dialog_ID == dialog_id);
            dialog.Time = time;
            dbAsync.UpdateAsync(dialog);
        }


        //удалить диалоги из БД
        public static void DeleteDialogs(string login)
        {
            if (dbAsync == null) ConnectToDBAsync();
            if (db == null) ConnectToDB();

            TableQuery<DialogsTable> dialogs = db.Table<DialogsTable>().Where((m) => m.Login == login);

            foreach (DialogsTable item in dialogs)
            {
                dbAsync.DeleteAsync(item);
            }
        }

    }
}