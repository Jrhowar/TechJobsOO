using System;
using TechJobs.Models;
using Microsoft.AspNetCore.Mvc;
using TechJobs.Data;
using TechJobs.ViewModels;

namespace TechJobs.Controllers
{
    public class JobController : Controller
    {

        // Our reference to the data store
        private static JobData jobData;

        static JobController()
        {
            jobData = JobData.GetInstance();
        }

        // The detail display for a given Job at URLs like /Job?id=17
        public IActionResult Index(int id)
        {

            Job theJob = jobData.Find(id);
            NewJobViewModel theRealJob = new NewJobViewModel();

            theRealJob.Name = theJob.Name;
            theRealJob.EmployerID = theJob.Employer.ID;
            theRealJob.CoreCompetencyID = theJob.CoreCompetency.ID;
            theRealJob.LocationID = theJob.Location.ID;
            theRealJob.PositionTypeID = theJob.PositionType.ID;

            return View(theJob);
        }

        public IActionResult New()
        {
            NewJobViewModel newJobViewModel = new NewJobViewModel();
            return View(newJobViewModel);
        }

        [HttpPost]
        public IActionResult New(NewJobViewModel newJobViewModel)
        {

            if (ModelState.IsValid)
            {
                Job newJob = new Job
                {
                    Name = newJobViewModel.Name,
                    Employer = jobData.Employers.Find(newJobViewModel.EmployerID),
                    Location = jobData.Locations.Find(newJobViewModel.LocationID),
                    CoreCompetency = jobData.CoreCompetencies.Find(newJobViewModel.CoreCompetencyID),
                    PositionType = jobData.PositionTypes.Find(newJobViewModel.PositionTypeID)
                };

                jobData.Jobs.Add(newJob);
                return Redirect(String.Format("/job?id={0}", newJob.ID));
            }

            return View(newJobViewModel);
        }
    }
}
