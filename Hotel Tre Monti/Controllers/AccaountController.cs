using Hotel_Tre_Monti.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hotel_Tre_Monti.Controllers
{
    public class AccaountController : Controller
    {
        // GET: Accaount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                // Verifica se le credenziali sono corrette
                if (IsValidUser(model.UserName, model.Password))
                {
                    // Se l'autenticazione ha successo, esegui il login manuale
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    // Reindirizza all'area protetta
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nome utente o password non validi.");
                }
            }

            // Se si arriva qui, qualcosa è andato storto, rimani sulla stessa pagina
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Esegui il logout manuale
            FormsAuthentication.SignOut();

            // Reindirizza alla pagina di login o ad altre azioni desiderate
            return RedirectToAction("Login", "Accaount");
        }

        private bool IsValidUser(string userName, string password)
        {

            // In questo esempio, confronto solo il nome utente e la password con un valore fisso
            return userName == "admin" && password == "password";
        }
    }
}