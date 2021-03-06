using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange,Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Everyday chores");
      Category secondCategory = new Category("Everyday chores");

      //Assert
      Assert.Equal(firstCategory,secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Everyday chores");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList,result);
    }

    [Fact]
    public void Test_Save_AssignIdToCategoryObject()
    {
      //Arrange
      Category testCategory = new Category("Everyday chores");
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];


      //Act
      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Everyday chores");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_GetTasks_RetrieveAllTasksWithCategory()
    {
      //Arrange
      Category testCategory = new Category("Everyday chores");
      testCategory.Save();

      DateTime myDate = new DateTime(1999,6,4);
      Task firstTask = new Task("Mow the lawn",testCategory.GetId(), myDate);
      firstTask.Save();

      Task secondTask = new Task("Do the Dishes",testCategory.GetId(), myDate);
      secondTask.Save();

      List<Task> testTaskList = new List<Task>{firstTask, secondTask};
      List<Task> result = testCategory.GetTask();

      Assert.Equal(testTaskList, result);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }

  }
}
