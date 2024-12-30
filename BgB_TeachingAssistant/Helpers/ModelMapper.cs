using BgB_TeachingAssistant.Models.PackageModels;
using Bgb_DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Models.Domain.PackageModels;

namespace BgB_TeachingAssistant.Helpers
{
    public static class ModelMapper
    {
        public static DisplayPackageInfoModel_Dto ToDto(DisplayPackageInfoModel package)
        {
            return new DisplayPackageInfoModel_Dto
            {
                PackageId = package.PackageId,
                StudentId = package.StudentId,
                PackageNumber = package.PackageNumber,
                IsCompleted = package.PackageCompleted,
                TotalLessons = package.LessonsAmount,
                RemainingLessons = package.OutstandingLessons,
                FinishedLessons = package.CompletedLessons
            };
        }

        public static DisplayPackageInfoModel ToEntity(DisplayPackageInfoModel_Dto packageDto)
        {
            return new DisplayPackageInfoModel
            {
                PackageId = packageDto.PackageId,
                StudentId = packageDto.StudentId,
                PackageNumber = packageDto.PackageNumber,
                PackageCompleted = packageDto.IsCompleted,
                LessonsAmount = packageDto.TotalLessons,
                OutstandingLessons = packageDto.RemainingLessons,
                CompletedLessons = packageDto.FinishedLessons
            };
        }
    }
}
