using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.ViewModel;
using Warehouse.ViewModel.Service;

namespace Warehouse.Creator.ViewModel
{
    public class BaseStepViewModel : BasePageViewModel
    {
        #region Private properties

        #endregion

        #region Public properties

        public EPageStep Step { get; protected set; } = EPageStep.Step1;

        #endregion

        #region Command

        public ICommand NextStepCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseStepViewModel(IApp app) : base(app)
        {
            NextStepCommand = new RelayCommand(NextStep);
        }

        protected virtual void NextStep()
        {
            
        }

        public void SetNextStep()
        {
            IBasePageViewModel? nextPage = null;

            EPageStep nextStep = (Step + 1);

            foreach(IBasePageViewModel pag in Application.Navigation.Pages)
            {
                if(pag is BaseStepViewModel step)
                {
                    if(step.Step == nextStep)
                    {
                        nextPage = step;
                        break;
                    }
                }
            }
            if(nextPage != null)
                Application.Navigation.SetPage(nextPage);
        }

        public void SetPrevStep()
        {
            IBasePageViewModel? nextPage = null;

            EPageStep nextStep = (Step - 1);

            foreach (IBasePageViewModel pag in Application.Navigation.Pages)
            {
                if (pag is BaseStepViewModel step)
                {
                    if (step.Step == nextStep)
                    {
                        nextPage = step;
                        break;
                    }
                }
            }
            if (nextPage != null)
                Application.Navigation.SetPage(nextPage);
        }

        #endregion
    }
}