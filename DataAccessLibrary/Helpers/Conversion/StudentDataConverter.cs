﻿using Bgb_DataAccessLibrary.Contracts;
using System.Data;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;

namespace Bgb_DataAccessLibrary.Helpers.Conversion
{
    public class StudentDataConverter
    {
        public List<IStudentPickerStudentModel> ConvertDataTableToStudentDTOList(DataTable dataTable)
        {
            var studentList = new List<IStudentPickerStudentModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                studentList.Add(new StudentPickerStudentModel
                {
                    StudentID = row.Field<int>("StudentID"),
                    StudentName = row.Field<string>("Name")
                });
            }

            return studentList;
        }
    }

}
