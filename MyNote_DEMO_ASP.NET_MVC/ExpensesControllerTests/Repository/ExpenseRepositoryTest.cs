

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyNote.Data;
using MyNote.Interface;
using MyNote.Models;
using MyNote.Repositories;


namespace MyNoteTest.Repository

{
   
    public class ExpenseRepositoryTest
    {
        public List<Expense> expenses;
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Expenses.CountAsync() <= 0)
            {
                databaseContext.Expenses.Add(new Expense { Id = 1, ExpensesName = "e1", Amount = 1 });
                databaseContext.Expenses.Add(new Expense { Id = 2, ExpensesName = "e1", Amount = 1 });
               

            }
            await databaseContext.SaveChangesAsync();
            return databaseContext;
        }
        [Fact]        
        public async void ExpenseRepository_Add_ValidInput_ReturnTrue()
        {
            //Arrange
            var expense = new Expense()
            {
                Id = 8,
                ExpensesName = "e1",
                Amount = 1
            };

            var dbContext = await GetDbContext(); //đóng vai trò như dbcontext thật
            var expenseRrpository = new ExpenseRepository(dbContext);
        
            //Act
            var result = expenseRrpository.Add(expense);

            //Assert
            result.Should().BeTrue();
         
        }

        [Fact]
        public async void ExpenseRepository_Add__InvalidInput_ThrowExeption()
        {
            //Arrange
            var expense = new Expense()
            {
                Id = 8,
                ExpensesName = null,
                Amount = 1
            };

            var dbContext = await GetDbContext(); //đóng vai trò như dbcontext thật
            var expenseRepository = new ExpenseRepository(dbContext);

            //Act

            //Assert
            Assert.Throws<DbUpdateException>(() => expenseRepository.Add(expense));

        }
        [Fact]
        public async void ExpenseRepository_GetById_InvalidInput_ReturnNull()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var expenseRrpository = new ExpenseRepository(dbContext);

            //Act
            var result = expenseRrpository.GetById(9);

            //Assert
            result.Should().NotBeNull();
            
        }
    }
       
}
