using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunch_App.Models;

namespace Lunch_App.Logic
{
    public static class DietartyIssueLogic
    {
        public static int SurveyDietaryIssues(SurveyEditVM survey)
        {
            int issues = 0;

            if (survey.Vegan)
            {
                issues += (int)DietaryIssues.Vegan;
            }
            if (survey.Vegetarian)
            {
                issues += (int)DietaryIssues.Vegetarian;
            }
            if (survey.GlutenFree)
            {
                issues += (int)DietaryIssues.GlutenFree;
            }
            if (survey.NutAllergy)
            {
                issues += (int)DietaryIssues.NutAllergy;
            }
            if (survey.ShellFishAllergy)
            {
                issues += (int)DietaryIssues.ShellFishAllergy;
            }
            if (survey.Kosher)
            {
                issues += (int)DietaryIssues.Kosher;
            }
            if (survey.Halaal)
            {
                issues += (int)DietaryIssues.Halaal;
            }
            if (survey.LactoseIntolerant)
            {
                issues += (int)DietaryIssues.LactoseIntolerant;
            }

            return issues;
        }
    }
}