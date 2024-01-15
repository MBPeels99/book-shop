using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookShop
{
    internal class DatabaseControllers
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\CodingProjects\\Dot Net\\BookShop\\BookShop\\BookShop.mdf;Integrated Security=True";

        public ArrayList getAllBooks()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Title FROM books", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ArrayList books = new ArrayList();
                    while (reader.Read())
                    {
                        books.Add(reader.GetString(0));
                    }
                    return books;
                }
            }
        }

        public ArrayList getBookInfo(string title)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM books WHERE Title = @Title";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ArrayList bookInfo = new ArrayList();
                        while (reader.Read())
                        {
                            bookInfo.Add(reader.GetString(0));
                            bookInfo.Add(reader.GetString(1));
                            bookInfo.Add(reader.GetString(2));
                           
                            if (reader.IsDBNull(3))
                            {
                                bookInfo.Add((string)null);
                            } else
                            {
                                bookInfo.Add(reader.GetString(3));
                            }
                        }
                        return bookInfo;
                    }
                }
            }
        }

        public Boolean addNewBook(string ISBN, string Title, string Author, string Description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO books (ISBN, Title, Author, Description) VALUES (@ISBN, @Title, @Author, @Description)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN",ISBN);
                    command.Parameters.AddWithValue("@Title",Title);
                    command.Parameters.AddWithValue("@Author",Author);
                    command.Parameters.AddWithValue("@Description",Description);
                    int rowAffected = command.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }
        }

        public Boolean deleteBookFromDB(string isbn)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM books WHERE ISBN=@ISBN";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN",isbn);
                    int rowAffected = command.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        public Boolean updateBook(string isbn, string title, string author, string description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE books SET Title = @Title, Author = @Author, Description = @Description WHERE ISBN = @ISBN";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN", isbn);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Author", author);
                    command.Parameters.AddWithValue("@Description", description);
                    int rowAffected = command.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
