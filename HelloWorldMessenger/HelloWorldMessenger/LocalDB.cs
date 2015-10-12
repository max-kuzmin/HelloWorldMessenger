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
        [PrimaryKey]
        public long Id { get; set; }

        public string Login { get; set; }
        public int Dialog_ID { get; set; }
        public long Time { get; set; }
        public string Text { get; set; }
    }

    [Table("Dialogs")]
    public class DialogsTable
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }
        public long Time { get; set; }
        public string Members { get; set; }
    }


    public static class HelpersDB
    {

        static SQLiteConnection db = null;


        static void ConnectToDB()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HelloWorldDB.db3");
            db = new SQLiteConnection(dbPath);
            db.CreateTable<MessagesTable>();
            db.CreateTable<DialogsTable>();
        }

        public static List<MessageData> GetMessages(int dialog_id)
        {
            if (db == null) ConnectToDB();
            TableQuery<MessagesTable> messages = db.Table<MessagesTable>().Where((m) => m.Dialog_ID == dialog_id);

            List<MessageData> result = new List<MessageData>();

            foreach (MessagesTable item in messages)
            {
                result.Add(new MessageData(item.Id, item.Text, item.Login, item.Time));

            }
            return result;
        }



        public static void PutMessages(List<MessageData> messages, int dialog_id)
        {
            if (db == null) ConnectToDB();

            foreach (MessageData item in messages)
            {
                db.Insert(new MessagesTable() { Login = item.Login, Dialog_ID = dialog_id, Id = item.Id, Text = item.Text, Time = item.Time });

            }
        }



        public static void PutDialogs(List<DialogData> dialogs)
        {
            if (db == null) ConnectToDB();

            foreach (DialogData item in dialogs)
            {
                db.Insert(new DialogsTable() { Id = item.Id, Name = item.Name, Time = item.Time});

            }
        }


        public static List<DialogData> GetDialogs()
        {
            if (db == null) ConnectToDB();
            TableQuery<DialogsTable> dialogs = db.Table<DialogsTable>();

            List<DialogData> result = new List<DialogData>();

            foreach (DialogsTable item in dialogs)
            {
                result.Add(new DialogData(item.Id, item.Name, item.Members, item.Time));

            }
            return result;
        }


        public static void UpdateDialogTime(long dialog_id, long time)
        {
            if (db == null) ConnectToDB();

            DialogsTable dialog = db.Table<DialogsTable>().First((m) => m.Id == dialog_id);
            dialog.Time = time;
            db.Update(dialog);
        }


        public static void DeleteDialog(long dialog_id)
        {
            if (db == null) ConnectToDB();

            DialogsTable dialog = db.Table<DialogsTable>().First((m) => m.Id == dialog_id);
            db.Delete(dialog);
        }
    }
}