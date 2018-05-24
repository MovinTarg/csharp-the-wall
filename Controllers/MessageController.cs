using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using the_wall.Models;

namespace the_wall.Controllers
{
    public class MessageController : Controller
    {
        [HttpGet]
        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            string getMessages = $@"SELECT messages.id as messageID, messages.user_id, messages.message, messages.created_at, users.id, users.first_name, users.last_name 
                FROM messages 
                JOIN users ON messages.user_id = users.id 
                ORDER BY messages.created_at DESC";
            List<Dictionary<string, object>> allMessages = DbConnector.Query(getMessages);
            string getComments = $@"SELECT comments.id as commentID, comments.message_id, comments.user_id, comments.comment, comments.created_at, users.id, users.first_name, users.last_name 
                FROM comments 
                JOIN users ON comments.user_id = users.id 
                ORDER BY comments.created_at ASC";
            List<Dictionary<string, object>> allComments = DbConnector.Query(getComments);
            ViewBag.allMessages = allMessages;
            ViewBag.allComments = allComments;
            ViewBag.activeUser = HttpContext.Session.GetInt32("activeUser");
            return View("Dashboard");
        }
        [HttpPost]
        [Route("/message/PostMessage")]
        public IActionResult PostMessage(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Message NewMessage = new Message
                    {
                        messageText = model.messageText,
                    };
                string query = $"INSERT INTO messages (user_id, message, created_at, updated_at) VALUES ({HttpContext.Session.GetInt32("activeUser")}, '{NewMessage.messageText}', NOW(), NOW())";
                DbConnector.Execute(query);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }
        [Route("/DeleteMessage/{message_id}")]
        public IActionResult DeleteMessage(int message_id)
        {
            string comments_query = $"DELETE FROM comments WHERE message_id={message_id};";
            Console.WriteLine(comments_query);
            string message_query = $"DELETE FROM messages WHERE id={message_id};";
            Console.WriteLine(message_query);
            DbConnector.Execute(comments_query);
            DbConnector.Execute(message_query);
            return RedirectToAction("Dashboard");
        }
        [Route ("/PostComment/{message_id}")]
        public IActionResult PostComment(CommentViewModel model, int message_id)
        {
            if (ModelState.IsValid)
            {
                Comment NewComment = new Comment
                    {
                        commentText = model.commentText,
                    };
                string query = $"INSERT INTO comments (message_id, user_id, comment, created_at, updated_at) VALUES ({message_id}, {HttpContext.Session.GetInt32("activeUser")}, '{NewComment.commentText}', NOW(), NOW())";
                DbConnector.Execute(query);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }
    }
}
