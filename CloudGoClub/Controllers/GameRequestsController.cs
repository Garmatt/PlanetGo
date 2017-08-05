using CloudGoClub.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudGoClub.Controllers
{
    [Authorize]
    public class GameRequestsController : GoController
    {
        public GameRequestsController()
            : base()
        { }

        [HttpGet]
        public ActionResult Index()
        {
            var requests = GetRequests().Where(gr => gr.IsOpen).ToList();
            var acceptableRequests = requests.Where(CanJoin);
            if (!acceptableRequests.Any())
                return RedirectToAction("Create");

            return View(acceptableRequests.ToList());
        }

        [NonAction]
        private IQueryable<GameRequest> GetRequests()
        {
            return db.GameRequests
                .Include(gr => gr.Author.Rank)
                .Include(gr => gr.BoardSize)
                .Include(gr => gr.LowestOpponentRank)
                .Include(gr => gr.HighestOpponentRank)
                .Include(gr => gr.SpecificOpponent)
                .Include(gr => gr.RuleSet)
                .Include(gr => gr.Tournament);
        }

        [NonAction]
        private bool CanJoin(GameRequest gameRequest)
        {
            if (gameRequest.Author == CurrentUser)
                return false;

            if (gameRequest.SpecificOpponent != null && gameRequest.SpecificOpponent != CurrentUser)
                return false;

            //todo
            //if (CurrentUser.Rank < gameRequest.LowestOpponentRank || CurrentUser.Rank > gameRequest.HighestOpponentRank)
            //    return false;

            return true;
        }

        [HttpGet]
        public ActionResult MyRequests()
        {
            var requests = GetRequests().Where(gr => gr.IsOpen).ToList();
            var myRequests = requests.Where(gr => gr.Author == CurrentUser);
            return View(myRequests.ToList());
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            GameRequest gameRequest = db.GameRequests.Find(id);
            db.GameRequests.Remove(gameRequest);
            db.SaveChanges();
            return RedirectToAction("MyRequests");
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.BoardSizes = new SelectList(db.BoardSizes, "Id", "DisplayName");
            return View();
        }

        [HttpGet]
        public ActionResult Join(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.BoardSizes.Load();
            db.RuleSets.Load();

            GameRequest gameRequest = db.GameRequests.Find(id);
            if (gameRequest == null)
            {
                return HttpNotFound();
            }

            if (gameRequest.Author.Id == CurrentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);
            }

            Game game = db.Games.Create();
            game.GameRequest = gameRequest;
            game.Moves = 0;
            game.Turn = Color.BLACK; //todo
            if (gameRequest.ColorOfAuthor == Color.BLACK)
            {
                game.BlackPlayer = gameRequest.Author;
                game.BlackPlayerRank = gameRequest.Author.Rank;
                game.WhitePlayer = CurrentUser;
                game.WhitePlayerRank = CurrentUser.Rank;
            }
            else
            {
                game.WhitePlayer = gameRequest.Author;
                game.WhitePlayerRank = gameRequest.Author.Rank;
                game.BlackPlayer = CurrentUser;
                game.BlackPlayerRank = CurrentUser.Rank;
            }
            game.StartTimestamp = DateTime.Now;
            db.Games.Add(game);
            gameRequest.IsOpen = false;
            db.SaveChanges();
            return RedirectToAction("Details", "Games", new { id = game.GameRequestId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(
            CreateGameRequestViewModel viewModel
            //[Bind(Include = "Id,SgfString,BoardSizeId,BlackPlayerId,WhitePlayerId,BlackPlayerRankId,WhitePlayerRankId,CreatorId,CreationTimestamp,StartTimestamp,LastPlayedTimestamp,EndTimestamp,Moves,CompensationPoints,HandicapStones,Turn,GameResultId,TournamentId,Type")] Game game
            )
        {
            if (ModelState.IsValid)
            {
                GameRequest gameRequest = db.GameRequests.Create();
                gameRequest.Author = CurrentUser;
                gameRequest.CreationTimestamp = DateTime.Now;
                gameRequest.Type = GameType.CORRESPONDENCE;
                gameRequest.HandicapStones = 0;
                gameRequest.CompensationPoints = 6.5;
                gameRequest.RuleSet = db.RuleSets.Find(0);
                gameRequest.BoardSize = db.BoardSizes.Find(viewModel.BoardSizeId);
                gameRequest.IsOpen = true;
                gameRequest.HandicapStonesPlacement = HandicapStonesPlacement.FIXED;
                gameRequest.ColorOfAuthor = Color.BLACK;

                db.GameRequests.Add(gameRequest);
                db.SaveChanges();
                return RedirectToAction("MyRequests");
            }

            return View(viewModel);
        }
    }
}