using CloudGoClub.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudGoClub.Controllers
{
    public class GamesController : GoController
    {
        public GamesController()
            : base()
        { }

        [HttpGet]
        public ActionResult Index()
        {
            var games = db.Games
                .Include(g => g.BlackPlayer)
                .Include(g => g.BlackPlayerRank) //baco: diverso da BlackPlayer.Rank
                .Include(g => g.GameRequest.BoardSize)
                .Include(g => g.GameResult)
                .Include(g => g.WhitePlayer)
                .Include(g => g.WhitePlayerRank);
            return View(games.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return RedirectToAction("Index", "GameRequests");
        }

        [HttpGet]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    ViewBag.BoardSizes = new SelectList(db.BoardSizes, "Id", "DisplayName");
        //    return View();
        //}

        // POST: Games/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Create(
        //    CreateGameViewModel viewModel
        //    //[Bind(Include = "Id,SgfString,BoardSizeId,BlackPlayerId,WhitePlayerId,BlackPlayerRankId,WhitePlayerRankId,CreatorId,CreationTimestamp,StartTimestamp,LastPlayedTimestamp,EndTimestamp,Moves,CompensationPoints,HandicapStones,Turn,GameResultId,TournamentId,Type")] Game game
        //    )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Game game = db.Games.Create();
        //        game.Moves = 0;
        //        game.Turn = CloudGoClub.Models.Color.BLACK;
        //        game.Type = GameType.CORRESPONDENCE;
        //        game.HandicapStones = 0;
        //        game.CompensationPoints = 6.5;
        //        game.RuleSet = db.RuleSets.Find(0);
        //        game.BlackPlayer = CurrentUser;
        //        game.BoardSize = db.BoardSizes.Find(viewModel.BoardSizeId);
        //        game.Creator = game.BlackPlayer;
        //        game.BlackPlayerRank = game.BlackPlayer.Rank;
        //        game.CreationTimestamp = DateTime.Now;
        //        db.Games.Add(game);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(viewModel);
        //}

        //// GET: Games/Edit/5
        //public ActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Game game = db.Games.Find(id);
        //    if (game == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BlackPlayerId = new SelectList(db.Users, "Id", "Email", game.BlackPlayerId);
        //    ViewBag.BlackPlayerRankId = new SelectList(db.Ranks, "Id", "Id", game.BlackPlayerRankId);
        //    ViewBag.BoardSizeId = new SelectList(db.BoardSizes, "Id", "Id", game.BoardSizeId);
        //    ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", game.CreatorId);
        //    ViewBag.GameResultId = new SelectList(db.GameResults, "GameId", "GameId", game.GameResultId);
        //    ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Id", game.TournamentId);
        //    ViewBag.WhitePlayerId = new SelectList(db.Users, "Id", "Email", game.WhitePlayerId);
        //    ViewBag.WhitePlayerRankId = new SelectList(db.Ranks, "Id", "Id", game.WhitePlayerRankId);
        //    return View(game);
        //}

        //// POST: Games/Edit/5
        //// Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        //// Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,SgfString,BoardSizeId,BlackPlayerId,WhitePlayerId,BlackPlayerRankId,WhitePlayerRankId,CreatorId,CreationTimestamp,StartTimestamp,LastPlayedTimestamp,EndTimestamp,Moves,CompensationPoints,HandicapStones,Turn,GameResultId,TournamentId,Type")] Game game)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(game).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BlackPlayerId = new SelectList(db.Users, "Id", "Email", game.BlackPlayerId);
        //    ViewBag.BlackPlayerRankId = new SelectList(db.Ranks, "Id", "Id", game.BlackPlayerRankId);
        //    ViewBag.BoardSizeId = new SelectList(db.BoardSizes, "Id", "Id", game.BoardSizeId);
        //    ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", game.CreatorId);
        //    ViewBag.GameResultId = new SelectList(db.GameResults, "GameId", "GameId", game.GameResultId);
        //    ViewBag.TournamentId = new SelectList(db.Tournaments, "Id", "Id", game.TournamentId);
        //    ViewBag.WhitePlayerId = new SelectList(db.Users, "Id", "Email", game.WhitePlayerId);
        //    ViewBag.WhitePlayerRankId = new SelectList(db.Ranks, "Id", "Id", game.WhitePlayerRankId);
        //    return View(game);
        //}

        //// GET: Games/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Game game = db.Games.Find(id);
        //    if (game == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(game);
        //}

        //// POST: Games/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Game game = db.Games.Find(id);
        //    db.Games.Remove(game);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
