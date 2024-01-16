using MyNote.Models;

namespace MyNote.Interface
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAll();
        Task<Expense?> GetById(int id);

        bool Add(Expense expense);

        bool Update(Expense expense);

        bool Delete(Expense expense);

        bool Save();
    }
}
