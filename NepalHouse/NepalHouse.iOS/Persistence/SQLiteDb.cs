using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using NepalHouse.Persistence;
using SQLite;
using System.IO;
using Xamarin.Forms;
using NepalHouse.iOS.Persistence;

[assembly: Dependency(typeof(SQLiteDb))]

namespace NepalHouse.iOS.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Nepalhouse.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}