using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BgB_TeachingAssistant.Services
{
    public class PackageNavigationService
    {
        public int MaxPackageNumber { get; private set; } = 5;

        public int PreviousPackage(int currentPackage)
        {
            if (currentPackage > 0)
            {
                return currentPackage - 1;
            }
            else
            {
                MessageBox.Show("MinPackageNumber reached..");
                return currentPackage;
            }
        }

        public int NextPackage(int currentPackage)
        {
            if (currentPackage < MaxPackageNumber)
            {
                return currentPackage + 1;
            }
            else
            {
                MessageBox.Show("MaxPackageNumber reached..");
                return currentPackage;
            }
        }

        public bool CanExecutePreviousPackage(int currentPackage)
        {
            return currentPackage > 0;
        }

        public bool CanExecuteNextPackage(int currentPackage)
        {
            return currentPackage < MaxPackageNumber;
        }
    }

}
