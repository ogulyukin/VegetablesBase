using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;

namespace VegetablesBase
{
    class DbWorks
    {
        public static List<InfoClass> getColors(String connection)
        {
            var result = new List<InfoClass>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'GoodsColorTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    var color = new InfoClass();
                    color.ID = res.GetInt32("id");
                    color.Name = res.GetString("name");

                    result.Add(color);
                }
            }            
            return result;
        }
        public static List<InfoClass> getGoodTypes(String connection)
        {
            var result = new List<InfoClass>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'GoodsTypesTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    var gType = new InfoClass();
                    gType.ID = res.GetInt32("id");
                    gType.Name = res.GetString("name");

                    result.Add(gType);
                }
            }
            return result;
        }
        public static List<Good> getGood(String connection)
        {
            var result = new List<Good>();

            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = "SELECT * FROM 'GoodsTab'";
            using var query = new SqliteCommand(sql, db);
            using var res = query.ExecuteReader();
            if (res.HasRows)
            {
                while (res.Read())
                {
                    var good = new Good();
                    good.ID = res.GetInt32("id");
                    good.Name = res.GetString("name");
                    good.TypeId = res.GetInt32("typeId");
                    good.ColorId = res.GetInt32("colorId");
                    good.Calory = res.GetInt32("calory");
                    result.Add(good);
                }
            }
            return result;
        }
        public static int AddColor(string name, string connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'GoodsColorTab' WHERE name = '{name}'";
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'GoodsColorTab'(name) VALUES ('{name}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'GoodsColorTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
        public static int AddType(string name, string connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'GoodsTypesTab' WHERE name = '{name}'";
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'GoodsTypesTab'(name) VALUES ('{name}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'GoodsTypesTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
        public static int AddGood(string name, int typeId, int colorId, int calory, string  connection)
        {
            int result = 0;
            using var db = new SqliteConnection(connection);
            db.Open();
            var sql = $"SELECT id FROM 'GoodsTab' WHERE name = '{name}'"; 
            using var query01 = new SqliteCommand(sql, db);
            using var res01 = query01.ExecuteReader();
            if (res01.HasRows)
                return result;
            sql = $"INSERT INTO 'GoodsTab'(name, typeId, colorId, calory) VALUES ('{name}','{typeId}','{colorId}','{calory}');";
            var query02 = new SqliteCommand(sql, db);
            query02.ExecuteNonQuery();
            sql = $"SELECT id FROM 'GoodsTab' WHERE name = '{name}'";
            var query03 = new SqliteCommand(sql, db);
            using var res02 = query03.ExecuteReader();
            if (res02.HasRows)
            {
                res02.Read();
                result = res02.GetInt32("id");
            }
            return result;
        }
    }
}
