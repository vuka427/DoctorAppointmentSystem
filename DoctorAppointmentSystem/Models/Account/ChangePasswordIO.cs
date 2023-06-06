using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class ChangePasswordIO
    {
        LoginIO loginIO = new LoginIO();
        DBContext dbContext = new DBContext();

        public List<string> GetAuthQuestions(string username)
        {
            USER user = loginIO.GetUser(username);
            List<int?> authQuestionIDList = new List<int?>();
            List<string> authQuestions = new List<string>();
            authQuestionIDList.Add(user.PASSWORDRECOVERYQUE1);
            authQuestionIDList.Add(user.PASSWORDRECOVERYQUE2);
            authQuestionIDList.Add(user.PASSWORDRECOVERYQUE3);
            foreach(int id in authQuestionIDList)
            {
                SYSTEM_PARA para = dbContext.SYSTEM_PARA.Where(s => s.ID.Equals(id)).FirstOrDefault();
                string question = para.PARAVAL != "" ? para.PARAVAL : "404! Not found question!";
                authQuestions.Add(question);
            }
            return authQuestions;
        }
    }
}