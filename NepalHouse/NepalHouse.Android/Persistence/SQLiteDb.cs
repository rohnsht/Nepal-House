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
using NepalHouse.Persistence;
using SQLite;
using System.IO;
using Xamarin.Forms;
using NepalHouse.Droid.Persistence;

[assembly: Dependency(typeof(SQLiteDb))]

namespace NepalHouse.Droid.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "NepalHouse.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}