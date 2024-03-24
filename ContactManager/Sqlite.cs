using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;

namespace ContactManager
{
    public class Sqlite
    {
        public static List<ContactModel> LoadContact()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var o = cnn.Query<ContactModel>("select * from contact", new DynamicParameters());
                return o.ToList();
            }
        }

        public static ContactModel LoadContact(int Id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var o = cnn.Query<ContactModel>("select * from contact where id = " + Id, new DynamicParameters());
                return o.FirstOrDefault();
            }
        }

        public static void DeleteContact(int Id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from contact where id = " + Id);
            }
        }
        public static void UpdateContact(ContactModel contact)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update contact set FirstName = @FirstName, LastName = @LastName, Email =  @Email, Address = @Address where id = @Id", contact);
            }
        }
        public static void SaveContact(ContactModel contact)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into contact (FirstName, LastName, Email, Address) values (@FirstName, @LastName, @Email, @Address)", contact);
            }
        }

        public static string LoadConnectionString(string id = "Default")
        {
            string relativePath = @"Database\Database.db";
            string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string absolutePath = System.IO.Path.Combine(currentPath, relativePath);
            absolutePath = absolutePath.Remove(0, 6);//this code is written to remove file word from absolute path
            return string.Format("Data Source={0}", absolutePath);
        }

        public static string LoadDBDir(string id = "Default")
        {
            return Path.GetDirectoryName(LoadConnectionString().Replace("Data Source=",""));
        }
    }
}
