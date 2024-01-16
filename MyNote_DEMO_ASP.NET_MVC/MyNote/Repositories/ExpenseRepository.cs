using Microsoft.EntityFrameworkCore;
using MyNote.Data;
using MyNote.Interface;
using MyNote.Models;

namespace MyNote.Repositories
{
    public class ExpenseRepository : IExpenseRepository 
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Expense?> GetById(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }
        public async Task<IEnumerable<Expense>> GetAll()
        {
            return await _context.Expenses.ToListAsync();
        }
        public bool Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            return Save();
        }

        public bool Delete(Expense expense)
        {
            _context.Expenses.Remove(expense);
            return Save();
        }

        public bool Update(Expense expense)
        {
            _context.Expenses.Update(expense);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

       
    }
}
