namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Models;

    /// <summary>
    /// Provides Feedback management
    /// </summary>
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsController"/> class.
        /// </summary>
        /// <param name="feedbackService"> The feedbacks service. </param>
        public RequestsController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            var feedbacks = _feedbackService.Get().ConvertAll(f => new RequestsViewModel(f));
            return View(feedbacks);
        }

        /// <summary>
        /// Feedback details.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Details(int id)
        {
            var feedback = _feedbackService.GetDetails(id);

            if (feedback == null)
            {
                return HttpNotFound();
            }

            var feedbackModel = new RequestsViewModel(feedback);

            return View(feedbackModel);
        }

        /// <summary>
        /// Feedback reply.
        /// </summary>
        /// <param name="feedbackId"> feedback id</param>
        /// <param name="feedbackMessage"> feedback message</param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Reply(int feedbackId, string feedbackMessage)
        {
            try
            {
                _feedbackService.Reply(feedbackId, feedbackMessage);
            }
            catch (InvalidOperationException ex)
            {
                return View(
                    "ErrorPage",
                    CreateErrorReply(ex));
            }
            catch (MissingEntityException ex)
            {
                return View(
                    "ErrorPage",
                    CreateErrorReply(ex));
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Feedback Close.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Close(int id)
        {
            try
            {
                _feedbackService.Close(id);
            }
            catch (InvalidOperationException ex)
            {
               return View(
                   "ErrorPage",
                   CreateErrorReply(ex));
            }
            catch (MissingEntityException ex)
            {
                return View(
                    "ErrorPage",
                    CreateErrorReply(ex));
            }

            return RedirectToAction("Index");
        }

        private static OperationResultViewModel CreateErrorReply(Exception ex)
        {
            return new OperationResultViewModel
            {
                Message = ex.Message
            };
        }
    }
}