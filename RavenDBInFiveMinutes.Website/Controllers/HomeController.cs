﻿using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using RavenDBInFiveMinutes.Website.Models;

namespace RavenDBInFiveMinutes.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _documentSession;

        public HomeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        
        // GET: /Home/
        [ActionName("Index")]
        public ActionResult List(string message)
        {
            var movies = _documentSession.Query<Movie>()
                .Customize(x=>x.WaitForNonStaleResults())
                .Take(50).ToList(); //TODO paging :)

            ViewBag.Message = message;

            return View(movies);
        }

        // GET: /Home/Details/5
        public ActionResult Details(int id)
        {
            var movie = _documentSession.Load<Movie>(id);

            if (movie==null)
                return RedirectToAction("Index", new { message = string.Format("Movie {0} not found", id) });

            return View(movie);
        }

        // GET: /Home/Create
        public ActionResult Create()
        {
            return View();
        } 

        // POST: /Home/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
                return View();

            _documentSession.Store(movie);
            _documentSession.SaveChanges();

            return RedirectToAction("Index", new { message = string.Format("Created Movie {0}", movie.Title) });
        }
        
        // GET: /Home/Edit/5 
        public ActionResult Edit(int id)
        {
            var movie = _documentSession.Load<Movie>(id);

            if (movie == null)
                return RedirectToAction("Index", new { message = string.Format("Movie {0} not found", id) });

            return View(movie);
        }

        // POST: /Home/Edit/
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (!ModelState.IsValid)
                return View();
            
            _documentSession.Store(movie);
            _documentSession.SaveChanges();

            return RedirectToAction("Index", new { message = string.Format("Saved changes to Movie {0}", movie.Title) });
        }

        // GET: /Home/Delete/5
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var movie = _documentSession.Load<Movie>(id);

            if (movie == null)
                return RedirectToAction("Index", new { message = string.Format("Movie {0} not found", id) });
            
            return View(movie);
        }

        // POST: /Home/Delete/
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _documentSession.Delete(_documentSession.Load<Movie>(id));
            _documentSession.SaveChanges();

            return RedirectToAction("Index", new { message = string.Format("Deleted Movie with the Id {0}", id) });
        }
    }
}
