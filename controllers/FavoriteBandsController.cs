using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JudgeMyTaste.Data;
using JudgeMyTaste.Models;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
  public class FavoriteBandsController : Controller
  {
    private readonly JudgeMyTasteContext _context;

    public FavoriteBandsController(JudgeMyTasteContext context)
    {
      _context = context;
    }

    // GET: FavoriteBands
    public async Task<IActionResult> Index()
    {
      return View(await _context.FavoriteBands.ToListAsync());
    }

    // GET: FavoriteBands/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var favoriteBand = await _context.FavoriteBands
          .FirstOrDefaultAsync(m => m.Id == id);
      if (favoriteBand == null)
      {
        return NotFound();
      }

      return View(favoriteBand);
    }

    // GET: FavoriteBands/Create
    [Authorize]
    public IActionResult Create()
    {
      return View();
    }

    // POST: FavoriteBands/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] FavoriteBand favoriteBand)
    {
      if (ModelState.IsValid)
      {
        favoriteBand.EnteredOn = DateTime.Now;
        favoriteBand.EnteredBy = User.Identity.Name;
        _context.Add(favoriteBand);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(favoriteBand);
    }

    // GET: FavoriteBands/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var favoriteBand = await _context.FavoriteBands.FindAsync(id);
      if (favoriteBand == null)
      {
        return NotFound();
      }
      return View(favoriteBand);
    }

    // POST: FavoriteBands/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FavoriteBand favoriteBand)
    {
      if (id != favoriteBand.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          var existingBand = await _context.FavoriteBands.FindAsync(id);
          if(existingBand.EnteredBy != User.Identity.Name)
          {
            return Unauthorized();
          }
          existingBand.Name = favoriteBand.Name;
          _context.Update(existingBand);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!FavoriteBandExists(favoriteBand.Id))
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
      return View(favoriteBand);
    }

    // GET: FavoriteBands/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var favoriteBand = await _context.FavoriteBands
          .FirstOrDefaultAsync(m => m.Id == id);
      if (favoriteBand == null)
      {
        return NotFound();
      }

      return View(favoriteBand);
    }

    // POST: FavoriteBands/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var favoriteBand = await _context.FavoriteBands.FindAsync(id);
      _context.FavoriteBands.Remove(favoriteBand);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool FavoriteBandExists(int id)
    {
      return _context.FavoriteBands.Any(e => e.Id == id);
    }
  }
}
