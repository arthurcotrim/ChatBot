using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChatBot;
using ChatBot.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ChatBot.Controllers
{
    public class ChatModelsController : Controller
    {
        private readonly AppDbContext _context;

        public ChatModelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChatModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChatModel.ToListAsync());
        }

        // GET: ChatModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChatModel == null)
            {
                return NotFound();
            }

            var chatModel = await _context.ChatModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatModel == null)
            {
                return NotFound();
            }

            return View(chatModel);
        }

        // GET: ChatModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Response,Message")] ChatModel chatModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatModel);
        }

        // GET: ChatModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChatModel == null)
            {
                return NotFound();
            }

            var chatModel = await _context.ChatModel.FindAsync(id);
            if (chatModel == null)
            {
                return NotFound();
            }
            return View(chatModel);
        }

        // POST: ChatModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Response,Message")] ChatModel chatModel)
        {
            if (id != chatModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatModelExists(chatModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chatModel);
        }

        // GET: ChatModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChatModel == null)
            {
                return NotFound();
            }

            var chatModel = await _context.ChatModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatModel == null)
            {
                return NotFound();
            }

            return View(chatModel);
        }

        // POST: ChatModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.ChatModel == null)
            {
                return Problem("Entity set 'AppDbContext.ChatModel'  is null.");
            }
            var chatModel = await _context.ChatModel.FindAsync(id);
            if (chatModel != null)
            {
                _context.ChatModel.Remove(chatModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatModelExists(int? id)
        {
            return _context.ChatModel.Any(e => e.Id == id);
        }


        [HttpPost("api/chat")]
        public async Task<JsonResult> Chat(RequestModel request)
        {
            var chatResponse = await _context.Set<ChatModel>().Where(_ => _.Message.ToUpper().Contains(request.Message.ToUpper())).FirstOrDefaultAsync();

            ResponseModel? response;

            if (chatResponse != null)
                response = new()
                {
                    Response = chatResponse.Response
                };
            else
                response = new()
                {
                    Response = "Não entendemos sua pergunta"
                };

            return Json(response);
        }
    }
}

