using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	public class TrainerController : Controller
	{
		private readonly ITrainerService _trainerService;

		public TrainerController(ITrainerService trainerService)
		{
			_trainerService = trainerService;
		}

		#region Get All Trainers
		// GET: Trainer/Index
		public IActionResult Index()
		{
			var trainers = _trainerService.GetAllTrainers();
			return View(trainers);
		}
		#endregion

		#region Create Trainer
		public IActionResult Create()
		{
			return View();
		}

		// POST: Trainer/Create
		[HttpPost]
		public IActionResult CreateTrainer(CreateTrainerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(nameof(Create), model);
			}


			var result = _trainerService.CreateTrainer(model);

			if (result)
			{
				TempData["SuccessMessage"] = "Trainer created successfully!";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ModelState.AddModelError("", "Trainer with this email or phone already exists.");
				return View(model);
			}

		}
		#endregion

		#region Trainer Details
		// GET: Trainer/Details/5
		public IActionResult Details(int id)
		{
			var trainer = _trainerService.GetTrainerDetails(id);

			if (trainer == null)
			{
				TempData["ErrorMessage"] = "Trainer not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(trainer);
		}
		#endregion

		#region Edit Trainer
		public IActionResult Edit(int id)
		{
			var trainer = _trainerService.GetTrainerToUpdate(id);

			if (trainer == null)
			{
				TempData["ErrorMessage"] = "Trainer not found.";
				return RedirectToAction(nameof(Index));
			}

			var updateModel = new UpdateTrainerViewModel
			{
				Name = trainer.Name,
				Email = trainer.Email,
				Phone = trainer.Phone,
				Specialties = trainer.Specialties,
				BuildingNumber = trainer.BuildingNumber,
				City = trainer.City,
				Street = trainer.Street
			};

			return View(updateModel);
		}

		// POST: Trainer/Edit/5
		[HttpPost]
		public IActionResult Edit(int id, UpdateTrainerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = _trainerService.UpdateTrainerDetails(model, id);

			if (result)
			{
				TempData["SuccessMessage"] = "Trainer updated successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to update trainer.";
			}

			return RedirectToAction(nameof(Index));

		}
		#endregion

		#region Delete Trainer
		// GET: Trainer/Delete/5
		public IActionResult Delete(int id)
		{
			var trainer = _trainerService.GetTrainerDetails(id);

			if (trainer == null)
			{
				TempData["ErrorMessage"] = "Trainer not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(trainer);
		}

		// POST: Trainer/DeleteConfirmed
		[HttpPost]
		public IActionResult DeleteConfirmed(int id)
		{
			var result = _trainerService.RemoveTrainer(id);

			if (result)
			{
				TempData["SuccessMessage"] = "Trainer deleted successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to delete trainer";
			}


			return RedirectToAction(nameof(Index));
		}
		#endregion
	}
}