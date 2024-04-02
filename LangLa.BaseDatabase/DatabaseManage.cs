using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LangLa.IO;
using LangLa.SqlConnection;
using MySql.Data.MySqlClient;

namespace LangLa.BaseDatabase
{
    public class DatabaseManage<T> where T : class, new()
    {
        private static string GetKeyColumnName()
        {
            PropertyInfo _keyProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => prop.GetCustomAttribute<KeyAttribute>() != null);
            
            return _keyProperty != null ? _keyProperty.Name : null;
        }

        private static object GetKeyValue(T entity)
        {
            PropertyInfo _keyProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => prop.GetCustomAttribute<KeyAttribute>() != null);
            
            return _keyProperty != null ? _keyProperty.GetValue(entity) : null;
        }

        public static List<T> GetAll()
        {
            List<T> result = new List<T>();
            string sqlCommand = string.Format("SELECT * FROM {0};", typeof(T).Name);
            MySqlConnection con = Connection.getConnection();
            MySqlCommand cmd = null;
            MySqlDataReader read = null;

            try
            {
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = sqlCommand;
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    T item = new T();
                    PropertyInfo[] properties = typeof(T).GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        if (!read.IsDBNull(read.GetOrdinal(property.Name)))
                        {
                            object value = null;

                            if (property.PropertyType == typeof(int))
                            {
                                value = read.GetInt32(property.Name);
                            }
                            else if (property.PropertyType == typeof(string))
                            {
                                value = read.GetString(property.Name);
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                value = read.GetInt16(property.Name) != 0;
                            }

                            property.SetValue(item, value);
                        }
                    }

                    result.Add(item);
                }
            }
            catch (Exception e)
            {
                Util.ShowErr(e);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd?.Dispose();
                read?.DisposeAsync();
            }
            
            Util.ShowInfo(string.Format("Load {0} DB success ({1})", typeof(T).Name.Replace("_", " "), result.Count));
            
            return result;
        }

        public static T GetById(int id)
        {
            T result = null;
            string keyColumnName = GetKeyColumnName();
            if (keyColumnName == null)
            {
                throw new InvalidOperationException("Không tìm thấy thuộc tính được đánh dấu là khóa chính.");
            }

            string sqlCommand = string.Format("SELECT * FROM {0} WHERE {1} = @Id;", typeof(T).Name, keyColumnName);
            MySqlConnection con = Connection.getConnection();
            MySqlCommand cmd = null;
            MySqlDataReader read = null;

            try
            {
                con.Open();
                cmd = new MySqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@Id", id);
                read = cmd.ExecuteReader();

                if (read.Read())
                {
                    result = new T();
                    PropertyInfo[] properties = typeof(T).GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        if (!read.IsDBNull(read.GetOrdinal(property.Name)))
                        {
                            object value = read[property.Name];
                            property.SetValue(result, value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Util.ShowErr(e);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd?.Dispose();
                read?.DisposeAsync();
            }

            return result;
        }

        public static bool Insert(T entity)
        {
            string columnNames = GetColumnNames();
            string columnValues = GetColumnValues(entity);
            string sqlCommand = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", typeof(T).Name, columnNames,
                columnValues);
            MySqlConnection con = Connection.getConnection();
            MySqlCommand cmd = null;

            try
            {
                con.Open();
                cmd = new MySqlCommand(sqlCommand, con);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Util.ShowErr(e);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd?.Dispose();
            }
        }

        public static bool Update(T entity)
        {
            string keyColumnName = GetKeyColumnName();
            if (keyColumnName == null)
            {
                throw new InvalidOperationException("Không tìm thấy thuộc tính được đánh dấu là khóa chính.");
            }

            string sqlCommand = string.Format("UPDATE {0} SET {1} WHERE {2} = @Id;", typeof(T).Name,
                GetUpdateValues(entity), keyColumnName);
            MySqlConnection con = Connection.getConnection();
            MySqlCommand cmd = null;

            try
            {
                con.Open();
                cmd = new MySqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@Id", GetKeyValue(entity));
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Util.ShowErr(e);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd?.Dispose();
            }
        }

        public static bool Delete(T entity)
        {
            string keyColumnName = GetKeyColumnName();

            if (keyColumnName == null)
            {
                throw new InvalidOperationException("Không tìm thấy thuộc tính được đánh dấu là khóa chính.");
            }

            string sqlCommand = string.Format("DELETE FROM {0} WHERE {1} = @Id;", typeof(T).Name, keyColumnName);
            MySqlConnection con = Connection.getConnection();
            MySqlCommand cmd = null;

            try
            {
                con.Open();
                cmd = new MySqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@Id", GetKeyValue(entity));
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Util.ShowErr(e);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd?.Dispose();
            }
        }

        private static string GetColumnNames()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> columnNames = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                columnNames.Add(property.Name);
            }

            return string.Join(", ", columnNames);
        }

        private static string GetColumnValues(T entity)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> columnValues = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(entity);
                if (value != null)
                {
                    columnValues.Add($"@{property.Name}");
                }
                else
                {
                    columnValues.Add("NULL");
                }
            }

            return string.Join(", ", columnValues);
        }

        private static string GetUpdateValues(T entity)
        {
            string keyName = GetKeyColumnName();
            
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> updateValues = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name != keyName)
                {
                    object value = property.GetValue(entity);
                    if (value != null)
                    {
                        updateValues.Add(string.Format("{0} = @{0}", property.Name));
                    }
                    else
                    {
                        updateValues.Add(string.Format("{0} = NULL", property.Name));
                    }
                }
            }

            return string.Join(", ", updateValues);
        }
    }
}