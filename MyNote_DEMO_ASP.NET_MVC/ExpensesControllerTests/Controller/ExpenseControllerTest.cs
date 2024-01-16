
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyNote.Controllers;
using MyNote.Data;
using MyNote.Interface;
using MyNote.Models;
using System.Net;


namespace MyNoteTest

{
    public class ExpenseControllerTest
    {


        ExpenseController _controller;
        Mock<IExpenseRepository> mock;
        public ExpenseControllerTest()
        {   //dependency
            mock = new Mock<IExpenseRepository>();
            _controller = new ExpenseController(mock.Object);
        }


        [Fact]
        public async void ExpenseController_Index_NoneInput_ReturnWell_WithExistedDatabase()
        {
            //purpose: test index method in ExpenseController 
            //input: none
            //expected: ViewResult 
            //          Number of Expenses in list <-> >= 0
            //pass criterias: return index view, there is expenses showed up or empty table
            //fail criterias: exception view or nothing view
            //test procedure:   1. open website
            //                  2. click the "expenses item" button
            //                  3. expense table is show up
            //STATUS:       TEST SUCCESSFULL

            //Arrange
            var expenses = new List<Expense>()
            {
                new Expense() { Id = 1, Amount = 1, ExpensesName = "1"},
                new Expense() { Id = 2, Amount = 2, ExpensesName = "2"}
            };
            //configurate method
            mock.Setup(x => x.GetAll()).ReturnsAsync(expenses);

            //Act
            var result = await _controller.Index();
            var viewResult = (ViewResult)result;
            var model = viewResult.Model as List<Expense>;
            //Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expenses, viewResult.Model);
            Assert.Equal(2, model.Count);

            //Fluent Assertion
            result.Should().BeOfType<ViewResult>();

        }


        [Fact]
        public async void ExpenseController_Create_NoneAgrument_ReturnCreateView()
        {
            //purpose: test create HttpGet method in ExpenseController 
            //input: none
            //expected: ViewResult
            //pass criterias: return Create view, there is form show up
            //fail criterias: 404
            //test procedure:   1. open website
            //                  2. click the "expenses item" button
            //                  3.  click the "Create new item" button
            //                  4. Form is showed up
            //STATUS:       TEST SUCCESSFULL

            //Arrange

            //Act
            var result = _controller.Create();
            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void ExpenseController_CreatePost_ValidExpenseInput_ReturnWell()
        {
            //purpose: test create HttpPost method in ExpenseController 
            //input: Expense class
            //expected: ViewResult, 1 new Expense created
            //pass criterias: return Create view, there is table with new item showed up
            //fail criterias: no new item added
            //test procedure:   1. open website
            //                  2. click the "expenses item" button
            //                  3.  click the "Create new item" button
            //                  4. Form is showed up
            //                  5. Submit form and return to Index View
            //                  6. Observe a new item showed up
            //STATUS:       TEST SUCCESSFULL

            //Arrange
           

            var newExpense = new Expense()
            {
                Id = (new Random().Next(1, int.MaxValue)),
                Amount = 100,
                ExpensesName = "100"
            };
            //Arrange
            //make sure that expense is not null
            mock.Setup(x => x.Add(It.IsAny<Expense>()))
                .Callback((Expense expense) =>
            {
            Assert.NotNull(expense.ExpensesName);
            Assert.NotNull(newExpense.Amount);
            })
            .Returns(true);

            //Act
            var result = await _controller.CreatePost(newExpense);
            var viewModel = result;
            //Assert
            Assert.IsType<RedirectToActionResult>(result);
 
        }
    }
}
