using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange,Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionAreTheSame()
    {
      //Arrange, Act
      Date myDate = new Date(1999,6,4);
      Task firstTask = new Task("Mow the lawn", 1, myDate);
      Task secondTask = new Task("Mow the lawn", 1, myDate);

      //Assert
      Assert.Equal(firstTask,secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Date myDate = new Date(1999,6,4);
      Task testTask = new Task("Mow the lawn", 1, myDate);

      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      // List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testTask.GetId(), result[0].GetId());
    }

    [Fact]
    public void Test_Save_AssignIdToObject()
    {
      //Arrange
      Date myDate = new Date(1999,6,4);
      Task testTask = new Task("Mow the lawn", 1, myDate);

      //Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Date myDate = new Date(1999,6,4);
      Task testTask = new Task("Mow the lawn", 1, myDate);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }
    public void Dispose()
    {
      Task.DeleteAll();
    }

  }
}
