using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private DateTime _date;

    public Task(string Description, int categoryId, DateTime date, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _categoryId = categoryId;
      _date = date;
    }

    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool categoryIdEquality = (this.GetCategoryId() == newTask.GetCategoryId());
        bool dateEquality = (this.GetDate() == newTask.GetDate());
        return (idEquality && descriptionEquality && dateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public int GetCategoryId()
    {
      return _categoryId;
    }

    public DateTime GetDate()
    {
      return _date;
    }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public static List<Task> GetAll()
     {
       List<Task> allTasks = new List<Task>{};

       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT * FROM tasks ORDER BY cast([date] as datetime) asc;", conn);
       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         int taskId = rdr.GetInt32(0);
         string taskDescription = rdr.GetString(1);
         int categoryId = rdr.GetInt32(2);
         DateTime taskDate = rdr.GetDateTime(3);
         Task newTask = new Task(taskDescription, categoryId, taskDate, taskId);
         allTasks.Add(newTask);
       }

       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }

       return allTasks;
     }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description, category_id, date) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskCategoryId, @TaskDate);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();


      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@TaskCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();

      SqlParameter DateParameter = new SqlParameter();
      DateParameter.ParameterName = "@TaskDate";
      DateParameter.Value = this.GetDate();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(categoryIdParameter);
      cmd.Parameters.Add(DateParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
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

      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks where id = @TaskId;", conn);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      int foundCategoryId = 0;
      DateTime foundDate = new DateTime();

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundCategoryId = rdr.GetInt32(2);
        foundDate = rdr.GetDateTime(3);
      }

      Task foundTask = new Task(foundTaskDescription, foundCategoryId, foundDate, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
  }
}
