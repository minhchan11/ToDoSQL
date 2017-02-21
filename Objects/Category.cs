using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ToDoList
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string name, int id =0)
    {
      _id = id;
      _name = name;
    }

    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool idEquality = (this.GetId() == newCategory.GetId());
        bool nameEquality = (this.GetName() == newCategory.GetName());
        return (idEquality && nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public static List<Category> GetAll()
     {
       List<Category> allCategory = new List<Category>{};

       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         int categoryId = rdr.GetInt32(0);
         string categoryName = rdr.GetString(1);
         Category newCategory = new Category(categoryName, categoryId);
         allCategory.Add(newCategory);
       }

       if (rdr != null)
       {
         rdr.Close();
       }

       if (conn != null)
       {
         conn.Close();
       }

       return allCategory;
     }

     public void Save()
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);


       SqlParameter nameParameter = new SqlParameter();
       nameParameter.ParameterName = "@CategoryName";
       nameParameter.Value = this.GetName();
       cmd.Parameters.Add(nameParameter);
       SqlDataReader rdr = cmd.ExecuteReader();

       while (rdr.Read())
       {
         this._id = rdr.GetInt32(0);
       }

       if (rdr != null)
       {
         rdr.Close();
       }

       if (conn != null)
       {
         conn.Close();
       }
     }


     public static void DeleteAll()
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
       cmd.ExecuteNonQuery();
     }

     public static Category Find(int id)
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM categories where id = @CategoryId;",conn);

       SqlParameter CategoryIdParameter = new SqlParameter();
       CategoryIdParameter.ParameterName = "@CategoryId";
       CategoryIdParameter.Value = id.ToString();
       cmd.Parameters.Add(CategoryIdParameter);
       SqlDataReader rdr = cmd.ExecuteReader();

       int foundCategoryId = 0;
       string foundCategoryName = null;

       while(rdr.Read())
       {
         foundCategoryId = rdr.GetInt32(0);
         foundCategoryName = rdr.GetString(1);
       }

       Category foundCategory = new Category(foundCategoryName,foundCategoryId);

       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }

       return foundCategory;
     }

  }
}
