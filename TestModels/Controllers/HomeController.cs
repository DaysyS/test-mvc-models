using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestModels.Models;

namespace TestModels.Controllers {
	public class HomeController : Controller {
        BookContext db = new BookContext();

        public ActionResult Index()
        {
            return View(db.Books);
        }

        public ActionResult BookView(int id)
        {
            return View(db.Books.Find(id));
        }

        #region Edit Model

        [HttpGet]
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Add/Remove Model
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Допустим, нам пришло электронное письмо, в которое была внедрена картинка посредством тега:
        ///<img src = "http://адрес_нашего_сайта/Home/Delete/1" />
        /// итоге при открытии письма 1-я запись в таблице может быть удалена.
        /// Уязвимость касается не только писем, но может проявляться и в других местах, 
        /// но смысл один - GET-запрос к методу Delete несет потенциальную уязвимость.
        /// </summary>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}