using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNote.Data;
using MyNote.Interface;
using MyNote.Models;

namespace MyNote.Controllers
{
    public class ExpenseController : Controller 
    {

        private readonly IExpenseRepository _repo;

        public ExpenseController(IExpenseRepository context)
        {
            _repo = context;
        }
        public async Task<IActionResult> Index()
        {
            var ExList = await _repo.GetAll();
            return View(ExList);
        }

        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(Expense expense) 
        {
            if (ModelState.IsValid)
            {
                _repo.Add(expense);
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = await _repo.GetById(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(int Id)
        {
            var obj = await _repo.GetById(Id);
            if (obj == null)
            {
                return NotFound();
            }
            _repo.Delete(obj);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {

            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = await _repo.GetById(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Expense obj)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(obj);  
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
